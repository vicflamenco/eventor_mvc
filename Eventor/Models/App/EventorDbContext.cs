using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class EventorDbContext : DbContext
    {
        public EventorDbContext() : base("name=EventorDb")
        {
            Database.SetInitializer<EventorDbContext>(new DropCreateDatabaseIfModelChanges<EventorDbContext>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Participante> Participantes { get; set; }
    }
}