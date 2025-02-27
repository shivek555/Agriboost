using Microsoft.ML;
using Microsoft.ML.Data;
using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgriBoostAPI.Services
{
    public class PricePredictionService
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private readonly AppDbContext _context;

        public PricePredictionService(AppDbContext context)
        {
            _mlContext = new MLContext();
            _context = context;
            TrainModel();
        }

        private void TrainModel()
        {
            var priceData = _context.Prices.ToList();

            if (!priceData.Any())
            {
                throw new InvalidOperationException("❌ No training data available in the Price table.");
            }

            Console.WriteLine("✅ Training Data Loaded:");
            foreach (var data in priceData)
            {
                Console.WriteLine($"- {data.ProductType} ({data.Location}) - Year: {data.Year}, Price: {data.HistoricalPrice}");
            }

            float minYear = (float)priceData.Min(p => p.Year);
            float maxYear = (float)priceData.Max(p => p.Year);

            var trainingData = priceData
                .OrderBy(p => p.Year)
                .Select(p =>
                {
                    var prevYearPrice = (float)(priceData.FirstOrDefault(x => x.Year == p.Year - 1)?.HistoricalPrice ?? 0m);
                    var priceChange = prevYearPrice > 0 ? (float)(prevYearPrice * 0.1f) : 0f; // ✅ Changed to 10% increase assumption

                    return new PricePredictionInput
                    {
                        ProductType = p.ProductType,
                        Location = p.Location,
                        Year = (float)(p.Year - minYear),  // ✅ Fixed type conversion issue
                        PriceChange = priceChange,
                        HistoricalPrice = (float)p.HistoricalPrice
                    };
                }).ToList();

            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding(nameof(PricePredictionInput.ProductType))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(nameof(PricePredictionInput.Location)))
                .Append(_mlContext.Transforms.NormalizeMeanVariance(nameof(PricePredictionInput.Year)))
                .Append(_mlContext.Transforms.NormalizeMeanVariance(nameof(PricePredictionInput.PriceChange)))
                .Append(_mlContext.Transforms.Concatenate("Features",
                    nameof(PricePredictionInput.Year),
                    nameof(PricePredictionInput.PriceChange),
                    nameof(PricePredictionInput.ProductType),
                    nameof(PricePredictionInput.Location)))
                .Append(_mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(PricePredictionInput.HistoricalPrice)))
                .Append(_mlContext.Regression.Trainers.LbfgsPoissonRegression()); // ✅ Switched to a better model

            _model = pipeline.Fit(dataView);
            Console.WriteLine("🚀 Model Training Completed Successfully!");
        }

        public float PredictPrice(string productType, string location, int targetYear)
        {
            var priceData = _context.Prices.ToList();
            float minYear = (float)priceData.Min(p => p.Year);
            float maxYear = (float)priceData.Max(p => p.Year);

            if (targetYear > maxYear + 30)
            {
                Console.WriteLine($"⚠ Warning: Year {targetYear} is too far ahead. Limiting to {maxYear + 30}");
                targetYear = (int)(maxYear + 30);
            }

            var previousYearPrice = (float)(priceData.FirstOrDefault(x => x.Year == targetYear - 1)?.HistoricalPrice ?? 0m);
            var priceChange = previousYearPrice > 0 ? (float)(previousYearPrice * 0.1f) : 0f; // ✅ Updated assumption

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<PricePredictionInput, PricePredictionOutput>(_model);

            var prediction = predictionEngine.Predict(new PricePredictionInput
            {
                ProductType = productType,
                Location = location,
                Year = (float)(targetYear - minYear),
                PriceChange = priceChange,
                HistoricalPrice = 0f
            });

            if (float.IsNaN(prediction.PredictedPrice) || float.IsInfinity(prediction.PredictedPrice))
            {
                Console.WriteLine($"❌ Invalid prediction for {productType} in {location} for {targetYear}. Returning -1.");
                return -1;
            }

            Console.WriteLine($"📊 Prediction for {productType} in {location} for {targetYear}: {prediction.PredictedPrice}");
            return prediction.PredictedPrice;
        }
    }

    public class PricePredictionInput
    {
        [LoadColumn(0)] public string ProductType { get; set; }
        [LoadColumn(1)] public string Location { get; set; }
        [LoadColumn(2)] public float Year { get; set; }
        [LoadColumn(3)] public float PriceChange { get; set; }
        [LoadColumn(4)] public float HistoricalPrice { get; set; }
    }

    public class PricePredictionOutput
    {
        [ColumnName("Score")] public float PredictedPrice { get; set; }
    }
}
