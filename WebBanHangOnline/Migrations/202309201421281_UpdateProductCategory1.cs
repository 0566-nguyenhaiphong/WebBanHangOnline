namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductCategory1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductCategories", "Alias", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductCategories", "Alias", c => c.String(nullable: false, maxLength: 150));
        }
    }
}
