namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVoucher2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Voucher", "isUse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Voucher", "isUse");
        }
    }
}
