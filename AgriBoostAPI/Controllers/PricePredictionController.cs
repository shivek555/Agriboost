using Microsoft.AspNetCore.Mvc;
using AgriBoostAPI.Services;

namespace AgriBoostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricePredictionController : ControllerBase
    {
        private readonly PricePredictionService _priceService;

        public PricePredictionController(PricePredictionService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet("{productType}/{location}/{year}")]
        public ActionResult<float> GetPredictedPrice(string productType, string location, int year)
        {
            float predictedPrice = _priceService.PredictPrice(productType, location, year);
            return Ok(predictedPrice);
        }
    }
}
