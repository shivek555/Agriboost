using System;
using System.ComponentModel.DataAnnotations;

namespace AgriBoostAPI.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = "UPI";  // Default: UPI Payment
        public decimal Amount { get; set; }
        public string QRCodeUrl { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}