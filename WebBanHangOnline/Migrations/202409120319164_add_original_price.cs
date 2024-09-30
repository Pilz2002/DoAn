namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_original_price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Product", "OriginalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Product", "OriginalPrice");
        }
    }
}
