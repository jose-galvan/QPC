namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToQC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Defect", "LastModificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Defect", "LastModificationUser_UserId", c => c.Guid());
            AddColumn("dbo.Defect", "UserCreated_UserId", c => c.Guid());
            AddColumn("dbo.Product", "LastModificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Product", "LastModificationUser_UserId", c => c.Guid());
            AddColumn("dbo.Product", "UserCreated_UserId", c => c.Guid());
            AddColumn("dbo.QualityControl", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.QualityControl", "LastModificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.QualityControl", "LastModificationUser_UserId", c => c.Guid());
            AddColumn("dbo.QualityControl", "UserCreated_UserId", c => c.Guid());
            CreateIndex("dbo.Defect", "LastModificationUser_UserId");
            CreateIndex("dbo.Defect", "UserCreated_UserId");
            CreateIndex("dbo.Product", "LastModificationUser_UserId");
            CreateIndex("dbo.Product", "UserCreated_UserId");
            CreateIndex("dbo.QualityControl", "LastModificationUser_UserId");
            CreateIndex("dbo.QualityControl", "UserCreated_UserId");
            AddForeignKey("dbo.Defect", "LastModificationUser_UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.Defect", "UserCreated_UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.Product", "LastModificationUser_UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.Product", "UserCreated_UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.QualityControl", "LastModificationUser_UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.QualityControl", "UserCreated_UserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QualityControl", "UserCreated_UserId", "dbo.User");
            DropForeignKey("dbo.QualityControl", "LastModificationUser_UserId", "dbo.User");
            DropForeignKey("dbo.Product", "UserCreated_UserId", "dbo.User");
            DropForeignKey("dbo.Product", "LastModificationUser_UserId", "dbo.User");
            DropForeignKey("dbo.Defect", "UserCreated_UserId", "dbo.User");
            DropForeignKey("dbo.Defect", "LastModificationUser_UserId", "dbo.User");
            DropIndex("dbo.QualityControl", new[] { "UserCreated_UserId" });
            DropIndex("dbo.QualityControl", new[] { "LastModificationUser_UserId" });
            DropIndex("dbo.Product", new[] { "UserCreated_UserId" });
            DropIndex("dbo.Product", new[] { "LastModificationUser_UserId" });
            DropIndex("dbo.Defect", new[] { "UserCreated_UserId" });
            DropIndex("dbo.Defect", new[] { "LastModificationUser_UserId" });
            DropColumn("dbo.QualityControl", "UserCreated_UserId");
            DropColumn("dbo.QualityControl", "LastModificationUser_UserId");
            DropColumn("dbo.QualityControl", "LastModificationDate");
            DropColumn("dbo.QualityControl", "Status");
            DropColumn("dbo.Product", "UserCreated_UserId");
            DropColumn("dbo.Product", "LastModificationUser_UserId");
            DropColumn("dbo.Product", "LastModificationDate");
            DropColumn("dbo.Defect", "UserCreated_UserId");
            DropColumn("dbo.Defect", "LastModificationUser_UserId");
            DropColumn("dbo.Defect", "LastModificationDate");
        }
    }
}
