using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRM.Web.Models;

namespace CRM.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Prospect> Prospects { get; set; }
    public DbSet<Communication> Communications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuration des relations
        builder.Entity<Communication>()
            .HasOne(c => c.Contact)
            .WithMany(co => co.Communications)
            .HasForeignKey(c => c.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Communication>()
            .HasOne(c => c.Client)
            .WithMany(cl => cl.Communications)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Communication>()
            .HasOne(c => c.Supplier)
            .WithMany(s => s.Communications)
            .HasForeignKey(c => c.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Communication>()
            .HasOne(c => c.Prospect)
            .WithMany(p => p.Communications)
            .HasForeignKey(c => c.ProspectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
