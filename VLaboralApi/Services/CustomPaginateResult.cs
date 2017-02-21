using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VLaboralApi.Services
{
    public class CustomPaginateResult<T> where T : class
    {
      
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public int TotalPages { get; set; }

        public JArray Results { get; set; }
       
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