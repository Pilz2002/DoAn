namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_alias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_ProductCategory", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_ProductCategory", "Alias");
        }
    }
}
