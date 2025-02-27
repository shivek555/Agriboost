using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriBoostAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FarmerController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/farmer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Farmer>>> GetFarmers()
        {
            return await _context.Farmers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Farmer>> GetFarmer(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null) return NotFound();
            return farmer;
        }

        [HttpPost]
        public async Task<ActionResult<Farmer>> CreateFarmer(Farmer farmer)
        {
            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFarmer), new { id = farmer.Id }, farmer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarmer(int id, Farmer farmer)
        {
            if (id != farmer.Id) return BadRequest();
            _context.Entry(farmer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null) return NotFound();
            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
