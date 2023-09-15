namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategories", "CreatedBy", c => c.String());
            AddColumn("dbo.ProductCategories", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProductCategories", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProductCategories", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCategories", "ModifiedBy");
            DropColumn("dbo.ProductCategories", "ModifiedDate");
            DropColumn("dbo.ProductCategories", "CreatedDate");
            DropColumn("dbo.ProductCategories", "CreatedBy");
        }
    }
}
