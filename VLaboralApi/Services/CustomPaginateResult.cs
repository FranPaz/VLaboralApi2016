using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Services
{
    public class CustomPaginateResult<T> where T : class
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}