using Microsoft.AspNetCore.Mvc;
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
    }
}
