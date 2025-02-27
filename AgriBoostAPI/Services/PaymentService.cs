using System;
using System.IO;
using System.Threading.Tasks;
using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using SkiaSharp.QrCode;

namespace AgriBoostAPI.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GenerateQRCodeForOrder(int orderId, decimal amount)
        {
            string upiId = "upi-id@bank"; 
            string upiPaymentString = $"upi://pay?pa={upiId}&pn=AgriBoost&mc=0000&tid=123456&tr=TXN{orderId}&tn=Order Payment&am={amount}&cu=INR";

            var qrGenerator = new QrCodeGenerator();
            var qrCode = qrGenerator.CreateQrCode(upiPaymentString, ECCLevel.Q);

            using (var surface = SKSurface.Create(new SKImageInfo(250, 250)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                var qrRenderer = new QrCodeRenderer();
                qrRenderer.Render(qrCode, canvas, new SKPoint(125, 125), 100);

                using (var image = surface.Snapshot())
                {
                    using (var ms = new MemoryStream())
                    {
                        image.Encode(ms, SKEncodedImageFormat.Png, 100);
                        string qrCodeBase64 = Convert.ToBase64String(ms.ToArray());

                        var payment = new Payment
                        {
                            OrderId = orderId,
                            Amount = amount,
                            QRCodeUrl = $"data:image/png;base64,{qrCodeBase64}",
                            Status = "Pending"
                        };

                        _context.Payments.Add(payment);
                        await _context.SaveChangesAsync();
                        return payment;
                    }
                }
            }
        }
    }
}
