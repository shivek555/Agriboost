using Microsoft.AspNetCore.Mvc;
using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgriBoostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PriceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Price>>> GetPrices()
        {
            return await _context.Prices.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Price>> CreatePrice(Price price)
        {
            _context.Prices.Add(price);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPrices), new { id = price.Id }, price);
        }
    }
}
