namespace QPC.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixDatetimeIssueDateTimeToDatetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Defect", "CreateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Defect", "LastModificationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Product", "CreateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Product", "LastModificationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.FinalDesicion", "CreateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.FinalDesicion", "LastModificationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Instruction", "CreateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Instruction", "LastModificationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.QualityControl", "CreateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.QualityControl", "LastModificationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Log", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Log", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QualityControl", "LastModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QualityControl", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Instruction", "LastModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Instruction", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FinalDesicion", "LastModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FinalDesicion", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Product", "LastModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Product", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Defect", "LastModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Defect", "CreateDate", c => c.DateTime(nullable: false));
        }
    }
}
