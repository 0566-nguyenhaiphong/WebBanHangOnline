namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVoucher1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Voucher", "VoucherCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Voucher", "VoucherCode");
        }
    }
}
