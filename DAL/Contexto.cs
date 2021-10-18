using Microsoft.EntityFrameworkCore;
using Registro_Con_Detalle_2._0.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registro_Con_Detalle_2._0.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permisos> Permiso { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"data Source = data\rolescontrol.Db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Permisos>().HasData(
                new Permisos { PermisoId = 1, Nombre = "Usuario", Descripcion = "Para usuarios" },
                new Permisos { PermisoId = 2, Nombre = "Administrador", Descripcion = "Para administradores" });
        }
    }
}
