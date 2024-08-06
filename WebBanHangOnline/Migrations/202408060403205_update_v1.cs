namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_v1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_Category", "SeoTitle", c => c.String(maxLength: 150));
            AlterColumn("dbo.tbl_Category", "SeoDescription", c => c.String(maxLength: 250));
            AlterColumn("dbo.tbl_Category", "SeoKeywords", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_Category", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tbl_Category", "SeoDescription", c => c.String());
            AlterColumn("dbo.tbl_Category", "SeoTitle", c => c.String());
        }
    }
}
