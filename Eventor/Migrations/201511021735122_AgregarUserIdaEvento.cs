namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarUserIdaEvento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Evento", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Evento", "UserId");
        }
    }
}
