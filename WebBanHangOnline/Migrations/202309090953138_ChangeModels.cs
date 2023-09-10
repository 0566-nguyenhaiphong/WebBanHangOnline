namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeModels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Posts", "Alias", c => c.String(maxLength: 150));
            AlterColumn("dbo.tb_Posts", "Image", c => c.String(maxLength: 150));
            AlterColumn("dbo.tb_Posts", "SeoTitle", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "SeoDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_Posts", "SeoKeywords", c => c.String(maxLength: 250));
            AlterColumn("dbo.Product", "Alias", c => c.String(maxLength: 250));
            AlterColumn("dbo.Product", "ProductCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.Product", "Image", c => c.String(maxLength: 250));
            AlterColumn("dbo.Product", "SeoTitle", c => c.String(maxLength: 250));
            AlterColumn("dbo.Product", "SeoDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Product", "SeoKeywords", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "SeoKeywords", c => c.String());
            AlterColumn("dbo.Product", "SeoDescription", c => c.String());
            AlterColumn("dbo.Product", "SeoTitle", c => c.String());
            AlterColumn("dbo.Product", "Image", c => c.String());
            AlterColumn("dbo.Product", "ProductCode", c => c.String());
            AlterColumn("dbo.Product", "Alias", c => c.String());
            AlterColumn("dbo.tb_Posts", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tb_Posts", "SeoDescription", c => c.String());
            AlterColumn("dbo.tb_Posts", "SeoTitle", c => c.String());
            AlterColumn("dbo.tb_Posts", "Image", c => c.String());
            AlterColumn("dbo.tb_Posts", "Alias", c => c.String());
        }
    }
}
