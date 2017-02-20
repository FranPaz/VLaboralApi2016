using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Models;
using VLaboralApi.Services;
using VLaboralApi.ViewModels.Filtros;
using VLaboralApi.ViewModels.Profesionales;


namespace VLaboralApi.Controllers
{
    public class ProfesionalsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        private IQueryable<Profesional> Profesionales()
        {
            return db.Profesionals.Include(sr => sr.Subrubros.Select(r => r.Rubro));
        }

        // GET: api/Profesionals
        [ResponseType(typeof(CustomPaginateResult<Profesional>))]
        public IHttpActionResult GetProfesionals(int page, int rows)
        {
            try
            {
                var data = Utiles.Paginate(new PaginateQueryParameters(page, rows)
                    , Profesionales()
                    , order => order.OrderBy(c => c.Id));
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseType(typeof(Profesional))]
        public IHttpActionResult GetProfesional(int tipoIdentificacion, string valor)
        {
            var profesional = db.Profesionals
                .FirstOrDefault(e => e.IdentificacionesProfesional.Any(ie => ie.TipoIdentificacionProfesionalId == tipoIdentificacion & ie.Valor == valor));

            return Ok(profesional);
        }


        // GET: api/Profesionals/5
        [ResponseType(typeof(Profesional))]
        public IHttpActionResult GetProfesional(int id)
        {
            try
            {
                var profesional = db.Profesionals
                    .Where(p => p.Id == id)
                    .Include(s => s.Subrubros.Select(r => r.Rubro))
                    .Include(i => i.IdentificacionesProfesional.Select(ti => ti.TipoIdentificacionProfesional))
                    .Include(exp => exp.ExperienciasLaborales.Select(e => e.Empresa))                    
                    .Include(cur => cur.Cursos)
                    .Include(educ => educ.Educaciones)
                    .Include(educ => educ.Educaciones.Select(nivel => nivel.TipoNivelEstudio))
                    .Include(idioma => idioma.IdiomasConocidos.Select(idio => idio.Idioma))
                    .Include(idioma => idioma.IdiomasConocidos.Select(comp => comp.CompetenciaIdioma))
                    .FirstOrDefault();

                if (profesional == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(profesional);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Profesionals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfesional(int id, Profesional profesional)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != profesional.Id)
                {
                    return BadRequest();
                }

                //fpaz: obtengo la entidad profesional original guardada en la bd
                var profDb = db.Profesionals
                    .Where(p => p.Id == id)
                    .Include(s => s.Subrubros)  
                    .Include(i => i.IdentificacionesProfesional)
                    .FirstOrDefault();

                
                //fpaz: actualizo las propiedades escalares del objeto profesional que recibo como parametro
                db.Entry(profDb).CurrentValues.SetValues(profesional);

                #region fpaz: update de subrubros
                //fpaz: elimino los subrubros que ya no se hayan enviado en el array de subrubros modificados
                foreach (var dbSubRubro in profDb.Subrubros.ToList())
                {
                    //fpaz: para cada subrubro asociado actualmente al profesional en la bd
                    if (!profesional.Subrubros.Any(s => s.Id == dbSubRubro.Id)) //busco si en el array de subrubros enviados como parametros, alguno de los objetos subrubros coincide con el de la base de datos
                    {
                        //si no encuentro al subrubro de la bd en el array ingresado como parametro, elimino la relacion entre ese subrubro y el profesional
                        profDb.Subrubros.Remove(dbSubRubro);
                    }
                }

                //fpaz: agrego los nuevos subrubros enviados en el array de subrubros del profesional
                foreach (var prmSubRubro in profesional.Subrubros)
                {
                    //fpaz: para cada subrubro ingresado como parametro
                    if (!profDb.Subrubros.Any(s => s.Id == prmSubRubro.Id)) //busco si el subrubro ingresado como parametro en el cliente esta actualmente en el array de subrubros asociados al profesional
                    {
                        //si el subrubro no esta relacionado
                        var a = db.SubRubros.Find(prmSubRubro.Id); //obtengo el objeto subrubro (esto por que es una relacion M a M)
                        profDb.Subrubros.Add(a); //agrego el subrubro al array de subrubros del profesional
                    }
                }
                #endregion

                #region fpaz: update identificaciones del profesional
                //fpaz: elimino las identificaciones del profesional que no se hayan enviado en el array de identificaciones modificadas
                foreach (var dbIdent in profDb.IdentificacionesProfesional.ToList())
                {
                    //fpaz: para cada identificacion asociado actualmente al profesional en la bd
                    if (!profesional.IdentificacionesProfesional.Any(s => s.Id == dbIdent.Id)) //busco si en el array de identificaciones enviados como parametros, alguno de los objetos identificacion coincide con el de la base de datos
                    {
                        //si no encuentro la identificacion en el array ingresado como parametro, elimino la relacion entre esa identificacion y el profesional
                        db.IdentificacionesProfesional.Remove(dbIdent);
                    }
                }

                //fpaz: agrego o actualizo las identificaciones del profesional
                foreach (var prmIdent in profesional.IdentificacionesProfesional)
                {
                    var dbIdent = profDb.IdentificacionesProfesional.FirstOrDefault(s => s.Id == prmIdent.Id); //busco si la identificacion ingresada como parametro esta asociada al profesional en la bd
                    if (dbIdent != null && dbIdent.Id > 0)
                        // Update de la identificacion
                        db.Entry(dbIdent).CurrentValues.SetValues(prmIdent);
                    else
                        //agrego una nueva identificacion al profesional
                        profDb.IdentificacionesProfesional.Add(prmIdent);
                }

                #endregion


                db.SaveChanges();

                return Ok(profDb);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // POST: api/Profesionals
        [ResponseType(typeof(Profesional))]
        public IHttpActionResult PostProfesional(Profesional profesional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GuardarProfesional(profesional);

            return CreatedAtRoute("DefaultApi", new { id = profesional.Id }, profesional);
        }

        public void GuardarProfesional(Profesional profesional)
        {
            profesional.Domicilio = null; //sluna: null hasta que definamos bien esto.
            profesional.DomicilioId = null; //sluna: null hasta que definamos bien esto.

            db.Profesionals.Add(profesional);
            db.SaveChanges();
        }

        // DELETE: api/Profesionals/5
        [ResponseType(typeof(Profesional))]
        public IHttpActionResult DeleteProfesional(int id)
        {
            Profesional profesional = db.Profesionals.Find(id);
            if (profesional == null)
            {
                return NotFound();
            }

            db.Profesionals.Remove(profesional);
            db.SaveChanges();

            return Ok(profesional);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfesionalExists(int id)
        {
            return db.Profesionals.Count(e => e.Id == id) > 0;
        }


        [HttpPost]
        [Route("api/Profesionals/QueryOptions")]
        public IHttpActionResult QueryOptions(ProfesionalesOptionsBindingModel options)
        {

          dynamic filters = new JObject();

            if (options != null && options.Filters != null)
            {
                //the filter values should be unique 'display' strings 
                if (options.Filters.Contains(ProfesionalesFilterOptions.Rubros))
                {
                    filters.Rubros = JArray.FromObject( db.SubRubros
                        .Where(s=> s.Profesionales.Any())
                                                    .Select(a => new ValorFiltroViewModel()
                                                    {
                                                        Id = a.Id,
                                                        Valor = a.Id.ToString(),
                                                        Descripcion = a.Nombre,
                                                        Cantidad =  db.Profesionals
                                                                .Count(p => p.Subrubros.Any(s => s.Id == a.Id))
                                                    })
                                                    .ToList());
                }
                if (options.Filters.Contains(ProfesionalesFilterOptions.Valoraciones))
                {
                    var valoracionProfesional = new List<ValorFiltroViewModel>();
                    for (var i = 1; i < 6; i++)
                    {
                        valoracionProfesional.Add(new ValorFiltroViewModel()
                                                    {
                                                        Id = i,
                                                        Valor = i.ToString(),
                                                        Descripcion  = i.ToString(),
                                                        Cantidad = db.Profesionals.Count(p => p.ValoracionPromedio >= i )
                                                    } );             
                    }
                    filters.Valoraciones = JArray.FromObject( valoracionProfesional);
                }
                if (options.Filters.Contains(ProfesionalesFilterOptions.Ubicaciones))
                {
                    filters.Ubicaciones = JArray.FromObject( db.Ciudades
                      .Where(c => c.Domicilios
                          .Any(d => d.Profesionales
                              .Any()))
                      .Select(c => new ValorFiltroViewModel()
                      {
                          Id = c.Id,
                          Descripcion = c.Nombre,
                          Cantidad = db.Profesionals.Count(p => p.Domicilio.CiudadId == c.Id)
                      }).ToList());
                }

            }

            var orderByOptions = Enum.GetNames(typeof(ProfesionalesOrderByOptions));

            return Ok(
                new
                {
                    options = new
                    {
                        selectableFilters = filters,
                        allFilterTypes = Enum.GetNames(typeof(ProfesionalesFilterOptions)),
                        orderByOptions,
                    },
                    query = new
                    {
                        orderBy = "",
                        searchText = "",
                        rubros = new List<string>()
                    }
                });
        }

        [HttpPost]
        [Route("api/Profesionals/Search")]
        public async Task<IHttpActionResult> Search(ProfesionalesQueryBindingModel queryOptions)
        {
            if (queryOptions == null)
            {
                return BadRequest("no query options provided");
            }

            //create the initial query...
            var query = Profesionales();

            //for each query option if it has values add it to the query
            if (!string.IsNullOrEmpty(queryOptions.SearchText))
            {
                query = query.Where(p => p.Apellido.Contains(queryOptions.SearchText));
            }

            if (queryOptions.Rubros != null && queryOptions.Rubros.Any())
            {
                query = query.Where(p => p.Subrubros.Any(s => queryOptions.Rubros.Contains(s.Id)));
            }

            if (queryOptions.Valoraciones != null && queryOptions.Valoraciones.Any())
            {
                queryOptions.Valoraciones.Sort();
                var valMin = queryOptions.Valoraciones.FirstOrDefault();
                query = query.Where(p => p.ValoracionPromedio >= valMin);
            }

            if (queryOptions.Ubicaciones != null && queryOptions.Ubicaciones.Any())
            {
                query = query.Where(p => queryOptions.Ubicaciones.Contains((int) p.Domicilio.CiudadId));
            }

            query = CreateOrderByExpression(query, queryOptions.OrderBy);


            var data = Utiles.Paginate(new PaginateQueryParameters(queryOptions.Page, queryOptions.Rows), query);
            return Ok(data);
        }

        private IQueryable<Profesional> CreateOrderByExpression(IQueryable<Profesional> query, ProfesionalesOrderByOptions orderByoption)
        {
            switch (orderByoption)
            {
                case ProfesionalesOrderByOptions.NombreCompleto:
                    query = query.OrderBy(p => p.Apellido).OrderBy(p => p.Nombre);
                    break;
                case ProfesionalesOrderByOptions.Valoracion:
                    query = query.OrderBy(p => p.IdentidadVerificada);
                    break;
                default:
                    query = query.OrderBy(p => p.Apellido).OrderBy(p => p.Nombre);
                    break;
            }

            return query;
        }
    }
}