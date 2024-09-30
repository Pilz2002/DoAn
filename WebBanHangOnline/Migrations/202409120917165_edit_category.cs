namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_category : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Category", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Category", "Link");
        }
    }
}
