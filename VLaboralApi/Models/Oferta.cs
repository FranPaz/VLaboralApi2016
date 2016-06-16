using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string FechaInicioConvocatoria { get; set; }
        public string FechaFinConvocatoria { get; set; }
        public bool Publica { get; set; }

    }
}