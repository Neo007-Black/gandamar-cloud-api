using System;
using System.ComponentModel.DataAnnotations;

namespace GandamarCloudAPI.Models
{
    public class CloudUser
    {
        [Key]
        public string SyncId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Username { get; set; } = "";
        
        [Required]
        public string PasswordHash { get; set; } = "";
        
        [Required]
        public string Role { get; set; } = "Cashier";
    }
}
