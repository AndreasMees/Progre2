using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique constraints
            builder.Entity<Customer>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            builder.Entity<Customer>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            builder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNo)
                .IsUnique();

            builder.Entity<Vehicle>()
                .HasIndex(v => v.LicensePlate)
                .IsUnique();

            builder.Entity<OperationType>()
                .HasIndex(o => o.Name)
                .IsUnique();

            // Seed initial operation types
            builder.Entity<OperationType>().HasData(
                new OperationType { Id = 1, Name = "Maintenance" },
                new OperationType { Id = 2, Name = "Repair" },
                new OperationType { Id = 3, Name = "Cleaning" },
                new OperationType { Id = 4, Name = "Relocation" },
                new OperationType { Id = 5, Name = "Inspection" }
            );
        }
    }
}