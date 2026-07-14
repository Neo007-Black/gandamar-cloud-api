using System;
using System.ComponentModel.DataAnnotations;

namespace GandamarCloudAPI.Models
{
    public class CloudSupplier
    {
        [Key]
        public string SyncId { get; set; } = Guid.NewGuid().ToString();
        public int LocalSupplierId { get; set; }
        
        [Required]
        public string SupplierName { get; set; } = "";
        
        public string ContactPerson { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        
        // For Mobile App Login
        public string PasswordHash { get; set; } = ""; // We can generate a simple pin/hash
        
        public DateTime LastSynced { get; set; } = DateTime.UtcNow;
    }
}
