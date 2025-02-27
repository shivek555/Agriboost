using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriBoostAPI.Services
{
    public class FarmerService
    {
        private readonly AppDbContext _context;

        public FarmerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Farmer>> GetAllFarmers()
        {
            return await _context.Farmers.ToListAsync();
        }
    }
}
