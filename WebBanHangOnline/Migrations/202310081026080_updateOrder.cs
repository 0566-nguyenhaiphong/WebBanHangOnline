namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.tb_Order", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "Address", c => c.String(nullable: false));
            DropColumn("dbo.tb_Order", "Email");
        }
    }
}
