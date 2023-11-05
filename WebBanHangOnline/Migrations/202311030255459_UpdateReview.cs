namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Review", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Review", "UserName");
        }
    }
}
