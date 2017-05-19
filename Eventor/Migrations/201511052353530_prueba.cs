namespace Eventor.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prueba : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        EntradaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Entradas", t => t.EntradaId, cascadeDelete: true)
                .Index(t => t.EntradaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "EntradaId", "dbo.Entradas");
            DropIndex("dbo.Items", new[] { "EntradaId" });
            DropTable("dbo.Items");
        }
    }
}
