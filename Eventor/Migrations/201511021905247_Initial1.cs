namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Entrada", "FechaLimite");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entrada", "FechaLimite", c => c.DateTime(nullable: false));
        }
    }
}
