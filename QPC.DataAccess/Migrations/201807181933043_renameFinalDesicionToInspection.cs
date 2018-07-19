namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameFinalDesicionToInspection : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FinalDesicion", newName: "Inspection");
            RenameColumn(table: "dbo.QualityControl", name: "Desicion_Id", newName: "Inspection_Id");
            RenameIndex(table: "dbo.QualityControl", name: "IX_Desicion_Id", newName: "IX_Inspection_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.QualityControl", name: "IX_Inspection_Id", newName: "IX_Desicion_Id");
            RenameColumn(table: "dbo.QualityControl", name: "Inspection_Id", newName: "Desicion_Id");
            RenameTable(name: "dbo.Inspection", newName: "FinalDesicion");
        }
    }
}
