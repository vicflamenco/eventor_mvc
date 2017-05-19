namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prueba_Items : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "Precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Item", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pedido", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedido", "Total");
            DropColumn("dbo.Item", "Subtotal");
            DropColumn("dbo.Item", "Precio");
        }
    }
}
