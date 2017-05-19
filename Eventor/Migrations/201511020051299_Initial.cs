namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacto",
                c => new
                    {
                        ContactoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Apellido = c.String(nullable: false, maxLength: 50),
                        Teléfono = c.String(nullable: false),
                        Correo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContactoId);
            
            CreateTable(
                "dbo.Entrada",
                c => new
                    {
                        EntradaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cupo = c.Int(nullable: false),
                        CantidadMinima = c.Int(nullable: false),
                        CantidadMaxima = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false),
                        FechaLimite = c.DateTime(nullable: false),
                        EventoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EntradaId)
                .ForeignKey("dbo.Evento", t => t.EventoId, cascadeDelete: true)
                .Index(t => t.EventoId);
            
            CreateTable(
                "dbo.Evento",
                c => new
                    {
                        EventoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Inicio = c.DateTime(nullable: false),
                        Final = c.DateTime(nullable: false),
                        Lugar = c.String(maxLength: 200),
                        Organizador = c.String(maxLength: 50),
                        Acceso = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        ContactoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventoId)
                .ForeignKey("dbo.Contacto", t => t.ContactoId, cascadeDelete: true)
                .Index(t => t.ContactoId);
            
            CreateTable(
                "dbo.Participante",
                c => new
                    {
                        ParticipanteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Correo = c.String(nullable: false),
                        Estado = c.Int(nullable: false),
                        EventoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipanteId)
                .ForeignKey("dbo.Evento", t => t.EventoId, cascadeDelete: true)
                .Index(t => t.EventoId);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        EntradaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Pedido_PedidoId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Entrada", t => t.EntradaId, cascadeDelete: true)
                .ForeignKey("dbo.Pedido", t => t.Pedido_PedidoId)
                .Index(t => t.EntradaId)
                .Index(t => t.Pedido_PedidoId);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        PedidoId = c.Int(nullable: false, identity: true),
                        FechaHora = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PedidoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "Pedido_PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.Item", "EntradaId", "dbo.Entrada");
            DropForeignKey("dbo.Participante", "EventoId", "dbo.Evento");
            DropForeignKey("dbo.Entrada", "EventoId", "dbo.Evento");
            DropForeignKey("dbo.Evento", "ContactoId", "dbo.Contacto");
            DropIndex("dbo.Item", new[] { "Pedido_PedidoId" });
            DropIndex("dbo.Item", new[] { "EntradaId" });
            DropIndex("dbo.Participante", new[] { "EventoId" });
            DropIndex("dbo.Evento", new[] { "ContactoId" });
            DropIndex("dbo.Entrada", new[] { "EventoId" });
            DropTable("dbo.Pedido");
            DropTable("dbo.Item");
            DropTable("dbo.Participante");
            DropTable("dbo.Evento");
            DropTable("dbo.Entrada");
            DropTable("dbo.Contacto");
        }
    }
}
