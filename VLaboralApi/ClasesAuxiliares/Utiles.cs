using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VlaboralApi.Infrastructure;
using VLaboralApi.Models;
using VLaboralApi.Services;

namespace VLaboralApi.ClasesAuxiliares
{
    public class Utiles
    {
        protected internal static int? GetReceptorId(string tipoReceptor, string UserId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            
            switch (tipoReceptor)
            {
                case "profesional":
                    return
                        Convert.ToInt32(manager.GetClaims(UserId).FirstOrDefault(r => r.Type == "profesionalId").Value);
                case "empresa":
                    return Convert.ToInt32(manager.GetClaims(UserId).FirstOrDefault(r => r.Type == "empresaId").Value);
                case "administracion":
                    return null;
            }
            return null;
        }

        protected internal static string GetTipoReceptor(string UserId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            var usuarioId = UserId;
            var appUsertype = manager.GetClaims(usuarioId).FirstOrDefault(r => r.Type == "app_usertype");
            return appUsertype == null ? null : appUsertype.Value;
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
                Results = results
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

            var query = collection
                 .Skip((parameters.Page -1) * parameters.Rows) //sluna: -1 para manejar base 1
                 .Take(parameters.Rows);

            var result = new CustomPaginateResult<TEntity>()
            {
                PageSize = parameters.Rows,
                TotalRows = totalRows,
                TotalPages = totalPages,
                CurrentPage = parameters.Page,
                Results = query.ToList()
            };

            return result;
        }
    }
}