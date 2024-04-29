using Microsoft.EntityFrameworkCore;
using Project1.Models;
using System.Collections.Generic;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }

}