using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GandamarCloudAPI.Models
{
    public class CloudOnlineOrder
    {
        [Key]
        public string SyncId { get; set; } = Guid.NewGuid().ToString(); // Global ID
        public string OrderNumber { get; set; } = ""; // e.g. ORD-20231015-001
        
        public string CustomerName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string CustomerAddress { get; set; } = "";
        public string DeliveryCompany { get; set; } = "";
        
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = ""; // KPay, WavePay, COD
        public string PaymentTransactionId { get; set; } = "";
        
        public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Cancelled
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        public bool IsSyncedToDesktop { get; set; } = false; // Flag to tell desktop it has downloaded this
        
        public List<CloudOnlineOrderDetail> Details { get; set; } = new();
    }

    public class CloudOnlineOrderDetail
    {
        [Key]
        public int Id { get; set; }
        
        public string OrderSyncId { get; set; } = "";
        
        public string ProductSyncId { get; set; } = "";
        public string ProductName { get; set; } = ""; // Snapshot
        
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
