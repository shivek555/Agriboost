using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgriBoostAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Product)
                .Include(o => o.Seller)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Product)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PlaceOrder(Order order)
        {
            
            var buyer = await _context.Buyers.FindAsync(order.BuyerId);
            var product = await _context.Products.FindAsync(order.ProductId);
            var seller = await _context.Sellers.FindAsync(order.SellerId);

            if (buyer == null || product == null || seller == null)
            {
                return BadRequest(new { error = "Invalid BuyerId, ProductId, or SellerId." });
            }

            order.TotalPrice = product.Price * order.Quantity;
            order.Status = "Pending";

            order.Buyer = null;
            order.Product = null;
            order.Seller = null;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }


        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
