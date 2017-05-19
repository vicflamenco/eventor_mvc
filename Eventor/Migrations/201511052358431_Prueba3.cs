namespace Eventor.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prueba3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Eventoes", "ContactoId", "dbo.Contactoes");
            DropForeignKey("dbo.Entradas", "EventoId", "dbo.Eventoes");
            DropForeignKey("dbo.Participantes", "EventoId", "dbo.Eventoes");
            DropForeignKey("dbo.Items", "EntradaId", "dbo.Entradas");
            DropIndex("dbo.Entradas", new[] { "EventoId" });
            DropIndex("dbo.Eventoes", new[] { "ContactoId" });
            DropIndex("dbo.Participantes", new[] { "EventoId" });
            DropIndex("dbo.Items", new[] { "EntradaId" });
            DropTable("dbo.Entradas");
            DropTable("dbo.Eventoes");
            DropTable("dbo.Contactoes");
            DropTable("dbo.Participantes");
            DropTable("dbo.Items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        EntradaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.Participantes",
                c => new
                    {
                        ParticipanteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Correo = c.String(nullable: false),
                        Estado = c.Int(nullable: false),
                        EventoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipanteId);
            
            CreateTable(
                "dbo.Contactoes",
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
                "dbo.Eventoes",
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
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EventoId);
            
            CreateTable(
                "dbo.Entradas",
                c => new
                    {
                        EntradaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cupo = c.Int(nullable: false),
                        CantidadMinima = c.Int(nullable: false),
                        CantidadMaxima = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false),
                        EventoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EntradaId);
            
            CreateIndex("dbo.Items", "EntradaId");
            CreateIndex("dbo.Participantes", "EventoId");
            CreateIndex("dbo.Eventoes", "ContactoId");
            CreateIndex("dbo.Entradas", "EventoId");
            AddForeignKey("dbo.Items", "EntradaId", "dbo.Entradas", "EntradaId", cascadeDelete: true);
            AddForeignKey("dbo.Participantes", "EventoId", "dbo.Eventoes", "EventoId", cascadeDelete: true);
            AddForeignKey("dbo.Entradas", "EventoId", "dbo.Eventoes", "EventoId", cascadeDelete: true);
            AddForeignKey("dbo.Eventoes", "ContactoId", "dbo.Contactoes", "ContactoId", cascadeDelete: true);
        }
    }
}
