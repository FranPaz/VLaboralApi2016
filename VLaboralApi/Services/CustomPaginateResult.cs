using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VLaboralApi.Models;

namespace VLaboralApi.Services
{
    public class CustomPaginateResult<T> where T : class
    {
        //iafar: comento esto hasta que se solucione la busqueda
        //private VLaboral_Context db = new VLaboral_Context();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<T> Results { get; set; }

        //public CustomPaginateResult<T> Resultado(int page, int rows,DbSet<T> dbSet, IQueryable<T> filtroQueryable)
        //{


        //    var totalRows = db..Count(filtroQueryable);

        //    var totalPages = (int)Math.Ceiling((double)totalRows / rows);
        //    var results = db.Profesionals
        //        .OrderBy(o => o.Id)
        //        .Skip((page - 1) * rows) //SLuna: -1 Para manejar indice(1) en pagina
        //        .Take(rows)
        //        .ToList();

        //    var result = new CustomPaginateResult<T>()
        //    {
        //        PageSize = rows,
        //        TotalRows = totalRows,
        //        TotalPages = totalPages,
        //        CurrentPage = page,
        //        Results = results
        //    };
        //    return null;
        //}

       
    }

    public class PaginateQueryParameters
    {
        public int Page;
        public int Rows;

        public PaginateQueryParameters(int page, int rows)
        {
            Page = page;
            Rows = rows;
        }
    }
}