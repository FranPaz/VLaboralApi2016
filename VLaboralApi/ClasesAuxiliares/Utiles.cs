using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using VlaboralApi.Infrastructure;
using VLaboralApi.Models;
using VLaboralApi.Services;

namespace VLaboralApi.ClasesAuxiliares
{
    public class Utiles
    {
        protected internal static int? GetReceptorId(TiposUsuario tipoUsuario, string UserId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            
            switch (tipoUsuario)
            {
                case TiposUsuario.profesional:
                    return
                        Convert.ToInt32(manager.GetClaims(UserId).FirstOrDefault(r => r.Type == "profesionalId").Value);
                case TiposUsuario.empresa:
                    return 
                        Convert.ToInt32(manager.GetClaims(UserId).FirstOrDefault(r => r.Type == "empresaId").Value);
                case TiposUsuario.administracion:
                    return null;
            }
            return null;
        }


        public enum TiposUsuario
        {
            profesional, empresa, administracion
        }
        protected internal static TiposUsuario GetTipoUsuario(string UserId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            var usuarioId = UserId;
            var appUsertype = manager.GetClaims(usuarioId).FirstOrDefault(r => r.Type == "app_usertype");
            return (TiposUsuario) (appUsertype == null ? null : Enum.Parse(typeof(TiposUsuario), appUsertype.Value));
        }

        protected internal static int? GetProfesionalId(string UserId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            return Convert.ToInt32(manager.GetClaims(UserId).FirstOrDefault(r => r.Type == "profesionalId").Value);
        }

        protected internal static int? GetEmpresaId(string UserId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            return Convert.ToInt32(manager.GetClaims(UserId).FirstOrDefault(r => r.Type == "empresaId").Value);
        }

        public static CustomPaginateResult<TEntity> Paginate<TEntity>(PaginateQueryParameters parameters, IQueryable<TEntity> collection, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
           where TEntity : class
        {
            var totalRows = collection.Count();
            var totalPages = (int)Math.Ceiling((double)totalRows / parameters.Rows);
            
            var results = orderBy(collection)
                 .Skip((parameters.Page -1) * parameters.Rows) //sluna: -1 para manejar base 1
                 .Take(parameters.Rows)
                 .ToList();

            var result = new CustomPaginateResult<TEntity>()
            {
                PageSize = parameters.Rows,
                TotalRows = totalRows,
                TotalPages = totalPages,
                CurrentPage = parameters.Page,
                Results = JArray.FromObject(results)
            };

            return result;
        }

        public static CustomPaginateResult<TEntity> Paginate<TEntity>(PaginateQueryParameters parameters, IQueryable<TEntity> collection)
         where TEntity : class
        {
            if (parameters.Page <= 0)
            {
                parameters.Page = 1; 
            }
            if (parameters.Rows <= 0)
            {
                parameters.Rows = 10; //sluna: Esto debería estar parametrizado y accesible desde la BD
            }

            var totalRows = collection.Count();
            var totalPages = (int)Math.Ceiling((double)totalRows / parameters.Rows);

            var results = collection
                 .Skip((parameters.Page -1) * parameters.Rows) //sluna: -1 para manejar base 1
                 .Take(parameters.Rows);

            var result = new CustomPaginateResult<TEntity>()
            {
                PageSize = parameters.Rows,
                TotalRows = totalRows,
                TotalPages = totalPages,
                CurrentPage = parameters.Page,
                Results = JArray.FromObject(results.ToList())
            };

            return result;
        }
    }
}