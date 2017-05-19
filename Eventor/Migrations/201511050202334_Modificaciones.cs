namespace Eventor.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modificaciones : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.EntradaId)
                .ForeignKey("dbo.Eventoes", t => t.EventoId, cascadeDelete: true)
                .Index(t => t.EventoId);
            
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
                .PrimaryKey(t => t.EventoId)
                .ForeignKey("dbo.Contactoes", t => t.ContactoId, cascadeDelete: true)
                .Index(t => t.ContactoId);
            
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
                "dbo.Participantes",
                c => new
                    {
                        ParticipanteId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Correo = c.String(nullable: false),
                        Estado = c.Int(nullable: false),
                        EventoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipanteId)
                .ForeignKey("dbo.Eventoes", t => t.EventoId, cascadeDelete: true)
                .Index(t => t.EventoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Participantes", "EventoId", "dbo.Eventoes");
            DropForeignKey("dbo.Entradas", "EventoId", "dbo.Eventoes");
            DropForeignKey("dbo.Eventoes", "ContactoId", "dbo.Contactoes");
            DropIndex("dbo.Participantes", new[] { "EventoId" });
            DropIndex("dbo.Eventoes", new[] { "ContactoId" });
            DropIndex("dbo.Entradas", new[] { "EventoId" });
            DropTable("dbo.Participantes");
            DropTable("dbo.Contactoes");
            DropTable("dbo.Eventoes");
            DropTable("dbo.Entradas");
        }
    }
}
