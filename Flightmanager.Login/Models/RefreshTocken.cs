namespace Flightmanager.Login.Models;

public class RefreshTocken
{
    public required string Token { get; set; }
    public DateTime Created { get; } = DateTime.Now;
    public DateTime Expires { get; set; }
}