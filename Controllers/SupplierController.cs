using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GandamarCloudAPI.Data;
using GandamarCloudAPI.Models;

namespace GandamarCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupplierController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            // Simple MVP login
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierName == req.SupplierName);
            if (supplier == null) return Unauthorized(new { message = "Supplier not found" });

            return Ok(new { message = "Login successful", supplierId = supplier.SyncId });
        }

        [HttpPost("add-stock")]
        public async Task<IActionResult> AddStock([FromBody] CloudStockEntry entry)
        {
            entry.SyncId = Guid.NewGuid().ToString();
            entry.EntryDate = DateTime.UtcNow;
            entry.IsSyncedToDesktop = false;

            _context.StockEntries.Add(entry);
            
            // Optionally update cloud product stock immediately
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.SyncId == entry.ProductSyncId);
            if (prod != null)
            {
                prod.StockQuantity += entry.QuantityAdded;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Stock added successfully" });
        }
    }

    public class LoginRequest
    {
        public string SupplierName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
