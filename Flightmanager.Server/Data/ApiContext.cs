using Microsoft.EntityFrameworkCore;
using Flightmanager.Server.Models;

namespace Flightmanager.Server.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
    }
}
