namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatev4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Category", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_News", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_Post", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_Product", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Product", "IsActive");
            DropColumn("dbo.tbl_Post", "IsActive");
            DropColumn("dbo.tbl_News", "IsActive");
            DropColumn("dbo.tbl_Category", "IsActive");
        }
    }
}
