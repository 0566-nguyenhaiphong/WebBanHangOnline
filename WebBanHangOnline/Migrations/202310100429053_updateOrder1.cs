namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrder1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Order", "isConfirm", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "isConfirm", c => c.Int(nullable: false));
        }
    }
}
