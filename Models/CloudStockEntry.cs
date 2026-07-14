using System;
using System.ComponentModel.DataAnnotations;

namespace GandamarCloudAPI.Models
{
    public class CloudStockEntry
    {
        [Key]
        public string SyncId { get; set; } = Guid.NewGuid().ToString();
        public string ProductSyncId { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public int QuantityAdded { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        public bool IsSyncedToDesktop { get; set; } = false;
    }
}
