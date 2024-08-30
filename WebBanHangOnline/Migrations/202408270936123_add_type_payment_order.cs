namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_type_payment_order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Order", "TypeOfPayment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Order", "TypeOfPayment");
        }
    }
}
