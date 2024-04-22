using System.ComponentModel.DataAnnotations;
namespace Flightmanager.Login.Models
{
    public class UserDto
    {
        [Key]
        public required string Username { get; set; } = string.Empty;
        public required string PasswordHash { get; set; } = string.Empty;
        
    }
}
