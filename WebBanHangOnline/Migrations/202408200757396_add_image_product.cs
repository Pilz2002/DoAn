namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_image_product : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Product", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Product", "Image");
        }
    }
}
