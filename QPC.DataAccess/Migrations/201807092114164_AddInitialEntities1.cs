namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialEntities1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QualityControl", "Defect_Id", "dbo.Defect");
            DropForeignKey("dbo.QualityControl", "Product_Id", "dbo.Product");
            DropIndex("dbo.QualityControl", new[] { "Defect_Id" });
            DropIndex("dbo.QualityControl", new[] { "Product_Id" });
            RenameColumn(table: "dbo.QualityControl", name: "Defect_Id", newName: "DefectId");
            RenameColumn(table: "dbo.QualityControl", name: "Product_Id", newName: "ProductId");
            AlterColumn("dbo.Defect", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Defect", "Description", c => c.String(maxLength: 250));
            AlterColumn("dbo.Product", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Product", "Description", c => c.String(maxLength: 250));
            AlterColumn("dbo.QualityControl", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.QualityControl", "Description", c => c.String(maxLength: 250));
            AlterColumn("dbo.QualityControl", "DefectId", c => c.Int(nullable: false));
            AlterColumn("dbo.QualityControl", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.QualityControl", "ProductId");
            CreateIndex("dbo.QualityControl", "DefectId");
            AddForeignKey("dbo.QualityControl", "DefectId", "dbo.Defect", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QualityControl", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QualityControl", "ProductId", "dbo.Product");
            DropForeignKey("dbo.QualityControl", "DefectId", "dbo.Defect");
            DropIndex("dbo.QualityControl", new[] { "DefectId" });
            DropIndex("dbo.QualityControl", new[] { "ProductId" });
            AlterColumn("dbo.QualityControl", "ProductId", c => c.Int());
            AlterColumn("dbo.QualityControl", "DefectId", c => c.Int());
            AlterColumn("dbo.QualityControl", "Description", c => c.String());
            AlterColumn("dbo.QualityControl", "Name", c => c.String());
            AlterColumn("dbo.Product", "Description", c => c.String());
            AlterColumn("dbo.Product", "Name", c => c.String());
            AlterColumn("dbo.Defect", "Description", c => c.String());
            AlterColumn("dbo.Defect", "Name", c => c.String());
            RenameColumn(table: "dbo.QualityControl", name: "ProductId", newName: "Product_Id");
            RenameColumn(table: "dbo.QualityControl", name: "DefectId", newName: "Defect_Id");
            CreateIndex("dbo.QualityControl", "Product_Id");
            CreateIndex("dbo.QualityControl", "Defect_Id");
            AddForeignKey("dbo.QualityControl", "Product_Id", "dbo.Product", "Id");
            AddForeignKey("dbo.QualityControl", "Defect_Id", "dbo.Defect", "Id");
        }
    }
}
