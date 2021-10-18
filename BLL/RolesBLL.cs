﻿using Microsoft.EntityFrameworkCore;
using Registro_Con_Detalle_2._0.DAL;
using Registro_Con_Detalle_2._0.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Registro_Con_Detalle_2._0.BLL
{
    public class RolesBLL
    {
        public static bool Guardar(Roles roles)
        {
            if (!Existe(roles.RolId))
                return Insertar(roles);
            else
                return Modificar(roles);
        }
        private static bool Insertar(Roles roles)
        {
            bool paso = false;
            Contexto db = new Contexto();
            Permisos permi = new Permisos();
            try
            {
                if (db.Roles.Add(roles) != null)
                {
                    foreach (var item in roles.Detalle)
                    {
                        var permiso = db.Permiso.Find(Utilidades.ToInt(item.ToString()));
                        permi.VecesAsignado += 1;
                    }
                    paso = db.SaveChanges() > 0;
                }

            }
            catch (Exception)
            { throw; }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        private static bool Modificar(Roles roles)
        {
            bool paso = false;
            Contexto db = new Contexto();
            try
            {
                db.Database.ExecuteSqlRaw($"Delete FROM RolesDetalle Where RolId={roles.RolId}");

                foreach (var item in roles.Detalle)
                {
                    db.Entry(item).State = EntityState.Added;
                }

                db.Entry(roles).State = EntityState.Modified;
                paso = db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static bool Eliminar(int id)
        {
            bool paso = false;
            Contexto db = new Contexto();
            try
            {
                Roles roles = db.Roles.Find(id);

                if (Existe(id))
                {
                    db.Roles.Remove(roles);
                    paso = db.SaveChanges() > 0;
                }

            }
            catch (Exception)
            { throw; }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static Roles Buscar(int id)
        {
            Contexto db = new Contexto();
            Roles roles = new Roles();
            try
            {
                roles = db.Roles.Include(x => x.Detalle).Where(x => x.RolId == id).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return roles;
        }

        public static List<Roles> GetList(Expression<Func<Roles, bool>> expression)
        {
            List<Roles> Roles = new List<Roles>();
            Contexto db = new Contexto();
            try
            {
                Roles = db.Roles.Where(expression).ToList();
                db.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return Roles;
        }
        private static bool Existe(int id)
        {
            bool encontrado = false;
            Contexto db = new Contexto();
            try
            {
                encontrado = db.Roles.Any(x => x.RolId == id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return encontrado;
        }
    }
}
