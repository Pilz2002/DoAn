namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_v3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_Adv", "Tilte", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_Adv", "Description", c => c.String());
            AlterColumn("dbo.tbl_Adv", "Image", c => c.String());
            AlterColumn("dbo.tbl_Adv", "Link", c => c.String());
            AlterColumn("dbo.tbl_Category", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_Category", "Description", c => c.String());
            AlterColumn("dbo.tbl_Category", "SeoTitle", c => c.String());
            AlterColumn("dbo.tbl_Category", "SeoDescription", c => c.String());
            AlterColumn("dbo.tbl_Category", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tbl_News", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_Post", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_Product", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_ProductCategory", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_SystemSetting", "SettingValue", c => c.String());
            AlterColumn("dbo.tbl_SystemSetting", "SettingDescription", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_SystemSetting", "SettingDescription", c => c.String(maxLength: 4000));
            AlterColumn("dbo.tbl_SystemSetting", "SettingValue", c => c.String(maxLength: 4000));
            AlterColumn("dbo.tbl_ProductCategory", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.tbl_Product", "Title", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.tbl_Post", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.tbl_News", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.tbl_Category", "SeoKeywords", c => c.String(maxLength: 150));
            AlterColumn("dbo.tbl_Category", "SeoDescription", c => c.String(maxLength: 250));
            AlterColumn("dbo.tbl_Category", "SeoTitle", c => c.String(maxLength: 150));
            AlterColumn("dbo.tbl_Category", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.tbl_Category", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.tbl_Adv", "Link", c => c.String(maxLength: 500));
            AlterColumn("dbo.tbl_Adv", "Image", c => c.String(maxLength: 500));
            AlterColumn("dbo.tbl_Adv", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.tbl_Adv", "Tilte", c => c.String(nullable: false, maxLength: 150));
        }
    }
}
