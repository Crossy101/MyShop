namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24082019 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Products", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Category", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String(maxLength: 60));
        }
    }
}
