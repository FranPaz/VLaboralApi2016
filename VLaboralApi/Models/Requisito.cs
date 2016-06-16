using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Requisito
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public bool Excluyente { get; set; }

    }
}