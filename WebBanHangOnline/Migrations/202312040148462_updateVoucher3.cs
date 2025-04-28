namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVoucher3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Voucher", "isSave", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Voucher", "isApply", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Voucher", "isApply");
            DropColumn("dbo.tb_Voucher", "isSave");
        }
    }
}
