namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QPC.DataAccess.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QPC.DataAccess.ApplicationDbContext context)
        {
            context.Desicion.AddOrUpdate(d => d.Name,
                new Core.Models.Desicion { Name = "Acepted" },
                new Core.Models.Desicion { Name = "Rejected" },
                new Core.Models.Desicion { Name = "Rework" }
                );
        }
    }
}
