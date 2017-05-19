namespace Eventor.Migrations.Identity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class ConfigurationIdentity : DbMigrationsConfiguration<Eventor.Models.Identity.ApplicationDbContext>
    {
        public ConfigurationIdentity()
        {
            AutomaticMigrationsEnabled = false;
        }

        //Add-Migration Initial -ConfigurationTypeName ConfigurationIdentity
        //Update-Database -ConfigurationTypeName ConfigurationIdentity


        protected override void Seed(Eventor.Models.Identity.ApplicationDbContext context)
        {

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roles = { "Admin", "Organizador", "Participante" };

            foreach (string rol in roles)
                if (!RoleManager.RoleExists(rol))
                    RoleManager.Create(new IdentityRole(rol));


            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var admin = new ApplicationUser()
            {
                UserName = "admin@eventor.com",
                Name = "Admin",
                Email = "admin@eventor.com",
            };
            manager.Create(admin, "123456789");
            manager.AddToRole(admin.Id, "Admin");


            var organizador = new ApplicationUser()
            {
                UserName = "organizador@eventor.com",
                Name = "Organizador",
                Email = "organizador@eventor.com"
            };
            manager.Create(organizador, "123456789");
            manager.AddToRole(organizador.Id, "Organizador");


            var participante = new ApplicationUser()
            {
                UserName = "participante@eventor.com",
                Name = "Participante",
                Email = "participante@eventor.com",
            };
            manager.Create(participante, "123456789");
            manager.AddToRole(participante.Id, "Participante");
        }
    }
}
