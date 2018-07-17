namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogEntityAndFixQCRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Instruction", "QualityControl_Id", "dbo.QualityControl");
            DropIndex("dbo.Instruction", new[] { "QualityControl_Id" });
            RenameColumn(table: "dbo.Instruction", name: "QualityControl_Id", newName: "QualityControlId");
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ClassName = c.String(),
                        MethodName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        UserCreated_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserCreated_UserId)
                .Index(t => t.UserCreated_UserId);
            
            AlterColumn("dbo.Instruction", "QualityControlId", c => c.Int(nullable: false));
            CreateIndex("dbo.Instruction", "QualityControlId");
            AddForeignKey("dbo.Instruction", "QualityControlId", "dbo.QualityControl", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Instruction", "QualityControlId", "dbo.QualityControl");
            DropForeignKey("dbo.Log", "UserCreated_UserId", "dbo.User");
            DropIndex("dbo.Log", new[] { "UserCreated_UserId" });
            DropIndex("dbo.Instruction", new[] { "QualityControlId" });
            AlterColumn("dbo.Instruction", "QualityControlId", c => c.Int());
            DropTable("dbo.Log");
            RenameColumn(table: "dbo.Instruction", name: "QualityControlId", newName: "QualityControl_Id");
            CreateIndex("dbo.Instruction", "QualityControl_Id");
            AddForeignKey("dbo.Instruction", "QualityControl_Id", "dbo.QualityControl", "Id");
        }
    }
}
