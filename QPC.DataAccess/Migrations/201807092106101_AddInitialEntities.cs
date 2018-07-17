namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Defect",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QualityControl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Defect_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Defect", t => t.Defect_Id)
                .ForeignKey("dbo.Product", t => t.Product_Id)
                .Index(t => t.Defect_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QualityControl", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.QualityControl", "Defect_Id", "dbo.Defect");
            DropIndex("dbo.QualityControl", new[] { "Product_Id" });
            DropIndex("dbo.QualityControl", new[] { "Defect_Id" });
            DropTable("dbo.QualityControl");
            DropTable("dbo.Product");
            DropTable("dbo.Defect");
        }
    }
}
