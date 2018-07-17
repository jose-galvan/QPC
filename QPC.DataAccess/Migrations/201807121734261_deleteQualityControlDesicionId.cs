namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteQualityControlDesicionId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QualityControl", "DesicionId", "dbo.FinalDesicion");
            DropIndex("dbo.QualityControl", new[] { "DesicionId" });
            RenameColumn(table: "dbo.QualityControl", name: "DesicionId", newName: "Desicion_Id");
            AlterColumn("dbo.QualityControl", "Desicion_Id", c => c.Int());
            CreateIndex("dbo.QualityControl", "Desicion_Id");
            AddForeignKey("dbo.QualityControl", "Desicion_Id", "dbo.FinalDesicion", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QualityControl", "Desicion_Id", "dbo.FinalDesicion");
            DropIndex("dbo.QualityControl", new[] { "Desicion_Id" });
            AlterColumn("dbo.QualityControl", "Desicion_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.QualityControl", name: "Desicion_Id", newName: "DesicionId");
            CreateIndex("dbo.QualityControl", "DesicionId");
            AddForeignKey("dbo.QualityControl", "DesicionId", "dbo.FinalDesicion", "Id", cascadeDelete: true);
        }
    }
}
