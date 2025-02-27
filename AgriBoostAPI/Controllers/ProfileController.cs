using AgriBoostAPI.Data;
using AgriBoostAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AgriBoostAPI.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize] // Requires authentication
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Check Profile Status (Returns "Incomplete" if missing)
        [HttpGet("status")]
        public IActionResult CheckProfileStatus()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.Find(userId);

            if (user == null) return Unauthorized();

            bool hasProfile = user.UserType switch
            {
                "Buyer" => _context.BuyerProfiles.Any(p => p.UserId == userId),
                "Seller" => _context.SellerProfiles.Any(p => p.UserId == userId),
                "Farmer" => _context.FarmerProfiles.Any(p => p.UserId == userId),
                _ => false
            };

            return hasProfile ? Ok(new { status = "Completed" }) : Ok(new { status = "Incomplete" });
        }

        // ✅ Complete Buyer Profile
        [HttpPost("buyer")]
        public async Task<IActionResult> CompleteBuyerProfile([FromBody] BuyerProfile profile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            profile.UserId = userId;

            _context.BuyerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Buyer profile completed successfully!" });
        }

        // ✅ Complete Seller Profile
        [HttpPost("seller")]
        public async Task<IActionResult> CompleteSellerProfile([FromBody] SellerProfile profile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            profile.UserId = userId;

            _context.SellerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Seller profile completed successfully!" });
        }

        // ✅ Complete Farmer Profile
        [HttpPost("farmer")]
        public async Task<IActionResult> CompleteFarmerProfile([FromBody] FarmerProfile profile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            profile.UserId = userId;

            _context.FarmerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Farmer profile completed successfully!" });
        }
    }
}
