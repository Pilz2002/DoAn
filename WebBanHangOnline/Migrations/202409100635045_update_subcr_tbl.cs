namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_subcr_tbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_Subscribe", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_Subscribe", "Email", c => c.String());
        }
    }
}
