
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class ConsecutivoController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        //GET api/Cons
        public List<Consecutivo> Get()
        {
            return ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
        }

        public IHttpActionResult Get(string id)
        {
            var list = ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
            var found = list.FirstOrDefault(c => c.Id.Equals(id));
            if (found == null) return NotFound();
            return Ok(found);
        }

        //POST: api/Cons
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult Post([FromBody] ConsecutivoTransaction consecutivo)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            for(int i = consecutivo.RangoInicial; i < consecutivo.RangoFinal + 1; i++)
            {
                db.Consecutivoes.Add(ConsecutivoCrypt.EncryptarConsecutivo(new Consecutivo
                {
                    Id = consecutivo.Prefijo + i.ToString(),
                    Prefijo = consecutivo.Prefijo,
                    Numero = i.ToString(),
                    Estado = consecutivo.Estado.ToString(),
                    Entidad = consecutivo.Entidad,
                }));
            }
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { consecutivo.RangoFinal }, consecutivo);
        }

        // DELETE: api/Cons?prefijo=AE
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult Delete(string prefijo)
        {
            //var listCon = ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
            prefijo = Crypt.Encryptar(prefijo.ToUpper());
            var consecutivos = db.Consecutivoes.Where(c => c.Prefijo.Contains(prefijo)).ToList();
            if (!consecutivos.Any()) return NotFound();
            foreach(var con in consecutivos)
                db.Consecutivoes.Remove(con);
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
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
        public bool Estado { get; set; }
        public string Entidad { get; set; }
    }
}
