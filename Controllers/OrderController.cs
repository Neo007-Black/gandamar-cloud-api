using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GandamarCloudAPI.Data;
using GandamarCloudAPI.Models;

namespace GandamarCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Details)
                .OrderByDescending(o => o.OrderDate)
                .Take(50)
                .ToListAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CloudOnlineOrder order)
        {
            order.SyncId = Guid.NewGuid().ToString();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";
            order.IsSyncedToDesktop = false;
            
            // Auto generate order number
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var count = _context.Orders.Count(o => o.OrderNumber.Contains(today)) + 1;
            order.OrderNumber = $"ONL-{today}-{count:D4}";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order created successfully", orderNumber = order.OrderNumber });
        }

        [HttpPut("{syncId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(string syncId, [FromBody] StatusUpdateRequest req)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.SyncId == syncId);
            if (order == null) return NotFound();

            order.Status = req.Status;
            order.IsSyncedToDesktop = false; // Mark to sync back to Desktop
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order status updated successfully" });
        }
    }

    public class StatusUpdateRequest
    {
        public string Status { get; set; } = "";
    }
}
