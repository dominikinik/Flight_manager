using System.ComponentModel.DataAnnotations;

namespace Flightmanager.Login.Models
{
    public class User
    {
        [Key] 
        [StringLength(200)] 
        public string Username { get; set; } = string.Empty;

        [StringLength(200)] 
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(200)] 
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpired { get; set; }
    }
}