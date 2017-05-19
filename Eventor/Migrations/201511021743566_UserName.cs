namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Evento", "UserName", c => c.String(nullable: false));
            DropColumn("dbo.Evento", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Evento", "UserId", c => c.String(nullable: false));
            DropColumn("dbo.Evento", "UserName");
        }
    }
}
