namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnecessaryPropsFromFinalDesicion : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FinalDesicion", "Name");
            DropColumn("dbo.FinalDesicion", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FinalDesicion", "Description", c => c.String(maxLength: 250));
            AddColumn("dbo.FinalDesicion", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
