using Flightmanager.Login.Models;
using Microsoft.EntityFrameworkCore;

namespace Flightmanager.Login.Data;

public class UserApiContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserApiContext(DbContextOptions<UserApiContext> options) : base(options)
    {
    }
}