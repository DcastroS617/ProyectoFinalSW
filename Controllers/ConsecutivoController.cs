using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class ConsecutivoController : ApiController
    {
        private readonly VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();

        //GET api/Cons
        public List<Consecutivo> Get()
        {
            return ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
        }

        [HttpGet]
        [Route("api/consecutivo/analytics")]
        public IHttpActionResult GetBasicAnalytics()
        {
            var consecutivoRepo = new ConsecutivoRepository();
            var analytics = consecutivoRepo.GetConsecutivoAnalytics();
            if (!analytics.Any())
            {
                return NotFound();
            }
            return Ok(analytics);
        }

        public IHttpActionResult Get(string id)
        {
            var list = ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
            var found = list.FirstOrDefault(c => c.Id.Equals(id));
            if (found == null) 
            {
                _error.SaveError("no se encontro consecutivo", "404");
                return NotFound();
            }

            return Ok(found);
        }

        //POST: api/Cons
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult Post([FromBody] ConsecutivoTransaction consecutivo)
        {            

            var consecutivoRepo = new ConsecutivoRepository();
            if (consecutivo.Prefijo == null || string.IsNullOrEmpty(consecutivo.Prefijo))
            {
                consecutivo.Prefijo = consecutivoRepo.CreatePrefijo();
            }

            if(consecutivo.RangoInicial <= 0)
            {
                consecutivo.RangoInicial = 200;
            }

            if(consecutivo.RangoFinal <= 0)
            {
                consecutivo.RangoFinal = 209;
            }

            for(int i = consecutivo.RangoInicial; i < consecutivo.RangoFinal + 1; i++)
            {
                db.Consecutivoes.Add(ConsecutivoCrypt.EncryptarConsecutivo(new Consecutivo
                {
                    Id = consecutivo.Prefijo + i.ToString(),
                    Descripcion = consecutivo.Descripcion.ToString(),
                    Entidad = consecutivo.Entidad,
                }));
            }

            db.SaveChanges();
            _bitacora.SaveBitacora(consecutivo.Entidad, "crear", "se inserto una seguidilla de consecutivos", consecutivo.Entidad);
            return CreatedAtRoute("DefaultApi", new { consecutivo.RangoFinal }, consecutivo);
        }
        [ResponseType(typeof(void))]
        public IHttpActionResult Put([FromUri]string prefijo, ConsecutivoTransaction consecutivo)
        {
           /* if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var consecutivos = ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
            var consecutivosFound = consecutivos.Where(c => c.Id.StartsWith(prefijo)).ToList();
            if (consecutivosFound.Count > consecutivo.RangoFinal)
            {
                _error.SaveError("el rango final fue menor al que estaba anteriormente", "400");
                return BadRequest();
            }
            for (int i = consecutivo.RangoInicial; i < consecutivo.RangoFinal + 1; i++)
            {
                    db.Consecutivoes.Add(ConsecutivoCrypt.EncryptarConsecutivo(new Consecutivo
                    {
                        Id = consecutivo.Prefijo + i.ToString(),
                        Descripcion = consecutivo.Descripcion.ToString(),
                        Entidad = consecutivo.Entidad,
                    }));
            }
            foreach(var con in consecutivosFound)
            {
                db.Consecutivoes.Remove(ConsecutivoCrypt.EncryptarConsecutivo(con));
            }
            try
            {
                db.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
                throw;
            }*/
            return Ok(consecutivo);

        }
        // DELETE: api/Cons?prefijo=AE
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult Delete([FromUri] string prefijo)
        {
            var listCon = ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
            prefijo = prefijo.ToUpper();
            var consecutivos = listCon.Where(c => c.Id.StartsWith(prefijo)).ToList();
            if (!consecutivos.Any()) 
            {
                _error.SaveError("no se encontro el consecutivo", "404");
                return NotFound();
            }
            consecutivos = ConsecutivoCrypt.EncryptarConsecutivos(consecutivos);

            db.Consecutivoes.RemoveRange(consecutivos);
            db.SaveChanges();
                       
            _bitacora.SaveBitacora(prefijo, "eliminar", "se elimino una seguidilla de consecutivos", prefijo);
            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(new { consecutivos });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class ConsecutivoTransaction
    {
        public int RangoInicial { get; set; }
        public int RangoFinal { get; set; }
        public string Prefijo { get; set; }
        public string Descripcion { get; set; }
        public string Entidad { get; set; }
    }
    
}
