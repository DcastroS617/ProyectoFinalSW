using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class ConsecutivoController : ApiController
    {
        private VVuelosEntity db = new VVuelosEntity();

        public List<Consecutivo> Get()
        {
            return ConsecutivoCrypt.DecryptarConsecutivos(db.Consecutivoes.ToList());
        }

        public IHttpActionResult Get(int id)
        {
            var result = ConsecutivoCrypt.DecryptarConsecutivo(db.Consecutivoes.Find(id));
            if(result == null) return NotFound();
            return Ok(result);
        }

        public IHttpActionResult Post([FromBody] Consecutivo consecutivo)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            db.Consecutivoes.Add(ConsecutivoCrypt.EncryptConsecutivo(consecutivo));
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new {consecutivo.Id}, consecutivo);
        }

        public IHttpActionResult Put(int id, [FromBody] Consecutivo consecutivo)
        {
            if(!ModelState.IsValid) return BadRequest();
            if(id != consecutivo.Id) return BadRequest();
            consecutivo = ConsecutivoCrypt.EncryptConsecutivo(consecutivo);
            db.Entry(consecutivo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }catch (DbUpdateConcurrencyException)
            {
                if (!FindConsecutivo(id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult Delete(int id)
        {
            var consecutivo = db.Consecutivoes.Find(id);
            if (consecutivo == null) return NotFound();
            db.Consecutivoes.Remove(consecutivo);
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
        private bool FindConsecutivo(int id) => db.Users.Count(e => e.Id == id) > 0;
    }
}
