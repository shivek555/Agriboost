using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriBoostAPI.Controllers
{
    [Route("api/buyers")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BuyerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Buyer>> RegisterBuyer(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBuyerById), new { id = buyer.Id }, buyer);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Buyer>>> GetBuyers()
        {
            return await _context.Buyers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Buyer>> GetBuyerById(int id)
        {
            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer == null)
                return NotFound();
            return buyer;
        }
    }
}
