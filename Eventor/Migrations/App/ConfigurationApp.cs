namespace Eventor.Migrations.App
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models.App;

    internal sealed class ConfigurationApp : DbMigrationsConfiguration<Eventor.Models.App.EventorDbContext>
    {
        public ConfigurationApp()
        {
            AutomaticMigrationsEnabled = false;
        }

        //Add-Migration Initial -ConfigurationTypeName ConfigurationApp
        //Update-Database -ConfigurationTypeName ConfigurationApp


        protected override void Seed(Eventor.Models.App.EventorDbContext context)
        {
            context.Contactos.Add(new Contacto()
            {
                Nombre = "Victor",
                Apellido = "Flamenco",
                Correo = "vicflamenco.7@gmail.com",
                Teléfono = "7016-1671",
            });
            context.SaveChanges();

            context.Eventos.Add(new Evento()
            {
                Nombre = "Mi Evento",
                Descripcion = "Descripción de prueba",
                Inicio = DateTime.Now,
                Final = DateTime.Now,
                Lugar = "Universidad Don Bosco",
                Organizador = "Escuela de Computación",
                Acceso = Acceso.Publico,
                UserName = "organizador@eventor.com",
                ContactoId = 1
            });
            context.SaveChanges();

            context.Entradas.Add(new Entrada()
            {
                Nombre = "Mi entrada",
                Cupo = 3,
                Descripcion = "Entrada para 3 personas, pagan 2",
                CantidadMinima = 1,
                CantidadMaxima = 10,
                Precio = 10m,
                EventoId = 1
            });
            context.SaveChanges();

            context.Participantes.Add(new Participante()
            {
                Nombre = "Carlos Romero",
                Correo = "carlos.romero@gmail.com",
                Estado = Estado.Invitado,
                EventoId = 1
            });
            context.SaveChanges();
        }
    }
}
