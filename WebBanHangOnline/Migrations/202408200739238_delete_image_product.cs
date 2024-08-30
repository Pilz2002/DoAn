namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_image_product : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_Product", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_Product", "Image", c => c.String());
        }
    }
}
