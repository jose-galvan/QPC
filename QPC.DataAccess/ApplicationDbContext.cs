using QPC.Core.Models;
using QPC.DataAccess.EntityFramework.Configuration;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace QPC.DataAccess
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext():base("QPCDb")
        {

        }
        //Identiy Entities
        internal IDbSet<User> Users { get; set; }
        internal IDbSet<Role> Roles { get; set; }
        internal IDbSet<ExternalLogin> Logins { get; set; }

        //Domain Entities
        public DbSet<QualityControl> QualityControls { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Desicion> Desicion { get; set; }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add(new ClaimConfiguration());
            modelBuilder.Properties<DateTime>()
                        .Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Products)
                .WithRequired(d => d.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
