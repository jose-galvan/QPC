namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetRelationBetweenDefectAndProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Defect", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.Defect", "ProductId");
            AddForeignKey("dbo.Defect", "ProductId", "dbo.Product", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Defect", "ProductId", "dbo.Product");
            DropIndex("dbo.Defect", new[] { "ProductId" });
            DropColumn("dbo.Defect", "ProductId");
        }
    }
}
