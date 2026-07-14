using System;
using System.ComponentModel.DataAnnotations;

namespace GandamarCloudAPI.Models
{
    public class CloudProduct
    {
        [Key]
        public string SyncId { get; set; } = Guid.NewGuid().ToString();
        public int LocalProductId { get; set; } // The ID from Desktop POS
        
        [Required]
        public string Barcode { get; set; } = "";
        
        [Required]
        public string ProductName { get; set; } = "";
        
        public string Category { get; set; } = "";
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int StockQuantity { get; set; }
        public string SupplierName { get; set; } = "";
        public DateTime LastSynced { get; set; } = DateTime.UtcNow;
    }
}
