namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDesicionEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Desicion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModificationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModificationUser_UserId = c.Guid(),
                        UserCreated_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.LastModificationUser_UserId)
                .ForeignKey("dbo.User", t => t.UserCreated_UserId)
                .Index(t => t.LastModificationUser_UserId)
                .Index(t => t.UserCreated_UserId);
            
            AddColumn("dbo.Inspection", "Desicion_Id", c => c.Int());
            CreateIndex("dbo.Inspection", "Desicion_Id");
            AddForeignKey("dbo.Inspection", "Desicion_Id", "dbo.Desicion", "Id");
            DropColumn("dbo.Inspection", "Desicion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inspection", "Desicion", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inspection", "Desicion_Id", "dbo.Desicion");
            DropForeignKey("dbo.Desicion", "UserCreated_UserId", "dbo.User");
            DropForeignKey("dbo.Desicion", "LastModificationUser_UserId", "dbo.User");
            DropIndex("dbo.Inspection", new[] { "Desicion_Id" });
            DropIndex("dbo.Desicion", new[] { "UserCreated_UserId" });
            DropIndex("dbo.Desicion", new[] { "LastModificationUser_UserId" });
            DropColumn("dbo.Inspection", "Desicion_Id");
            DropTable("dbo.Desicion");
        }
    }
}
