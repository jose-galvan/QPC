namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInstructionAndDesicion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinalDesicion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Desicion = c.Int(nullable: false),
                        Comments = c.String(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 250),
                        CreateDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        LastModificationUser_UserId = c.Guid(),
                        UserCreated_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.LastModificationUser_UserId)
                .ForeignKey("dbo.User", t => t.UserCreated_UserId)
                .Index(t => t.LastModificationUser_UserId)
                .Index(t => t.UserCreated_UserId);
            
            CreateTable(
                "dbo.Instruction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Comments = c.String(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 250),
                        CreateDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        LastModificationUser_UserId = c.Guid(),
                        UserCreated_UserId = c.Guid(),
                        QualityControl_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.LastModificationUser_UserId)
                .ForeignKey("dbo.User", t => t.UserCreated_UserId)
                .ForeignKey("dbo.QualityControl", t => t.QualityControl_Id)
                .Index(t => t.LastModificationUser_UserId)
                .Index(t => t.UserCreated_UserId)
                .Index(t => t.QualityControl_Id);
            
            AddColumn("dbo.QualityControl", "DesicionId", c => c.Int(nullable: false));
            CreateIndex("dbo.QualityControl", "DesicionId");
            AddForeignKey("dbo.QualityControl", "DesicionId", "dbo.FinalDesicion", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Instruction", "QualityControl_Id", "dbo.QualityControl");
            DropForeignKey("dbo.QualityControl", "DesicionId", "dbo.FinalDesicion");
            DropForeignKey("dbo.Instruction", "UserCreated_UserId", "dbo.User");
            DropForeignKey("dbo.Instruction", "LastModificationUser_UserId", "dbo.User");
            DropForeignKey("dbo.FinalDesicion", "UserCreated_UserId", "dbo.User");
            DropForeignKey("dbo.FinalDesicion", "LastModificationUser_UserId", "dbo.User");
            DropIndex("dbo.QualityControl", new[] { "DesicionId" });
            DropIndex("dbo.Instruction", new[] { "QualityControl_Id" });
            DropIndex("dbo.Instruction", new[] { "UserCreated_UserId" });
            DropIndex("dbo.Instruction", new[] { "LastModificationUser_UserId" });
            DropIndex("dbo.FinalDesicion", new[] { "UserCreated_UserId" });
            DropIndex("dbo.FinalDesicion", new[] { "LastModificationUser_UserId" });
            DropColumn("dbo.QualityControl", "DesicionId");
            DropTable("dbo.Instruction");
            DropTable("dbo.FinalDesicion");
        }
    }
}
