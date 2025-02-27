using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgriBoostAPI.Controllers
{
    [Route("api/sellers")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            return await _context.Sellers.Include(s => s.Products).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Seller>> CreateSeller(Seller seller)
        {
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSellers), new { id = seller.Id }, seller);
        }
    }
}
