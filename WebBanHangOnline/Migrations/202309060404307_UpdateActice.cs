namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "IsActice", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_News", "IsActice", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Posts", "IsActice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "IsActice", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "IsActice");
            DropColumn("dbo.tb_Posts", "IsActice");
            DropColumn("dbo.tb_News", "IsActice");
            DropColumn("dbo.tb_Category", "IsActice");
        }
    }
}
