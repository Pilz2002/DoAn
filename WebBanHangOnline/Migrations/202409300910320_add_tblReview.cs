namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tblReview : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tbl_Review", "ProductId");
            AddForeignKey("dbo.tbl_Review", "ProductId", "dbo.tbl_Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_Review", "ProductId", "dbo.tbl_Product");
            DropIndex("dbo.tbl_Review", new[] { "ProductId" });
        }
    }
}
