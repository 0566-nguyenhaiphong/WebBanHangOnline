namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVoucher : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Voucher", "PercentReduce", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Voucher", "PercentReduce", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
