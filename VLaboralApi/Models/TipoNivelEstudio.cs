﻿using System.Collections.Generic;

namespace VLaboralApi.Models
{
    public class TipoNivelEstudio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //fpaz: relacion 1 a m con estudios (muchos)
        public virtual ICollection<Estudio> Estudios { get; set; }
    }
}