namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_trangThai : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Order", "TrangThai", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Order", "TrangThai");
        }
    }
}
