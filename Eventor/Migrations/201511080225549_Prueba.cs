namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prueba : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Item", "Pedido_PedidoId", "dbo.Pedido");
            DropIndex("dbo.Item", new[] { "Pedido_PedidoId" });
            RenameColumn(table: "dbo.Item", name: "Pedido_PedidoId", newName: "PedidoId");
            AddColumn("dbo.Pedido", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Item", "PedidoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Item", "PedidoId");
            AddForeignKey("dbo.Item", "PedidoId", "dbo.Pedido", "PedidoId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "PedidoId", "dbo.Pedido");
            DropIndex("dbo.Item", new[] { "PedidoId" });
            AlterColumn("dbo.Item", "PedidoId", c => c.Int());
            DropColumn("dbo.Pedido", "UserName");
            RenameColumn(table: "dbo.Item", name: "PedidoId", newName: "Pedido_PedidoId");
            CreateIndex("dbo.Item", "Pedido_PedidoId");
            AddForeignKey("dbo.Item", "Pedido_PedidoId", "dbo.Pedido", "PedidoId");
        }
    }
}
