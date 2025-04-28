namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNameThongKes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ThongKes", newName: "tb_ThongKes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tb_ThongKes", newName: "ThongKes");
        }
    }
}
