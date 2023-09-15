namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ProductImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Image = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ProductImage", "ProductId", "dbo.Product");
            DropIndex("dbo.tb_ProductImage", new[] { "ProductId" });
            DropTable("dbo.tb_ProductImage");
        }
    }
}
