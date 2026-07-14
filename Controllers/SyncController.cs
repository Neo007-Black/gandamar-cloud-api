using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GandamarCloudAPI.Data;
using GandamarCloudAPI.Models;

namespace GandamarCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SyncController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("products")]
        public async Task<IActionResult> SyncProducts([FromBody] List<CloudProduct> products)
        {
            foreach (var p in products)
            {
                var existing = await _context.Products.FirstOrDefaultAsync(x => x.LocalProductId == p.LocalProductId);
                if (existing == null)
                {
                    _context.Products.Add(p);
                }
                else
                {
                    existing.ProductName = p.ProductName;
                    existing.Category = p.Category;
                    existing.Barcode = p.Barcode;
                    existing.CostPrice = p.CostPrice;
                    existing.SalePrice = p.SalePrice;
                    existing.StockQuantity = p.StockQuantity;
                    existing.SupplierName = p.SupplierName;
                    existing.LastSynced = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Products synced successfully" });
        }

        [HttpPost("suppliers")]
        public async Task<IActionResult> SyncSuppliers([FromBody] List<CloudSupplier> suppliers)
        {
            foreach (var s in suppliers)
            {
                var existing = await _context.Suppliers.FirstOrDefaultAsync(x => x.LocalSupplierId == s.LocalSupplierId);
                if (existing == null)
                {
                    _context.Suppliers.Add(s);
                }
                else
                {
                    existing.SupplierName = s.SupplierName;
                    existing.ContactPerson = s.ContactPerson;
                    existing.Phone = s.Phone;
                    existing.Address = s.Address;
                    existing.LastSynced = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Suppliers synced successfully" });
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetNewOrders()
        {
            // Desktop calls this to get orders placed via Mobile
            var orders = await _context.Orders
                .Include(o => o.Details)
                .Where(o => !o.IsSyncedToDesktop)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost("orders/mark-synced")]
        public async Task<IActionResult> MarkOrdersSynced([FromBody] List<string> orderSyncIds)
        {
            var orders = await _context.Orders.Where(o => orderSyncIds.Contains(o.SyncId)).ToListAsync();
            foreach (var o in orders)
            {
                o.IsSyncedToDesktop = true;
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Orders marked as synced" });
        }

        [HttpGet("stock-entries")]
        public async Task<IActionResult> GetNewStockEntries()
        {
            var entries = await _context.StockEntries.Where(e => !e.IsSyncedToDesktop).ToListAsync();
            return Ok(entries);
        }

        [HttpPost("stock-entries/mark-synced")]
        public async Task<IActionResult> MarkStockSynced([FromBody] List<string> entrySyncIds)
        {
            var entries = await _context.StockEntries.Where(e => entrySyncIds.Contains(e.SyncId)).ToListAsync();
            foreach (var e in entries)
            {
                e.IsSyncedToDesktop = true;
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Stock entries marked as synced" });
        }
    }
}
