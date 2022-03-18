using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class PuertaAeropuertoController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        // GET: api/PuertaAeropuerto
        public List<PuertaAeropuerto> GetPuertasAeropuerto()
        {
            return PuertaAeropuertoCrypt.DecryptPuertasAeropuerto(db.PuertaAeropuertoes.ToList());
        }

        // GET: api/PuertaAeropuerto/5
        [ResponseType(typeof(PuertaAeropuerto))]
        public IHttpActionResult GetPuertaAeropuerto(string id)
        {
            id = Crypt.Encryptar(id);
            var puertaAeropuerto = db.PuertaAeropuertoes.FirstOrDefault(p => p.Id.Equals(id));
            if (puertaAeropuerto == null)
                return NotFound();
            return Ok(PuertaAeropuertoCrypt.DecryptPuertaAeropuerto(puertaAeropuerto));
        }

        // PUT: api/PuertaAeropuerto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPuertaAeropuerto(string id, PuertaAeropuerto puertaAeropuerto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != puertaAeropuerto.Id)
                return BadRequest();

            puertaAeropuerto = PuertaAeropuertoCrypt.EncryptPuertaAeropuerto(puertaAeropuerto);
            db.Entry(puertaAeropuerto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertaAeropuertoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PuertaAeropuerto
        [ResponseType(typeof(PuertaAeropuerto))]
        public IHttpActionResult PostPuertaAeropuerto(PuertaAeropuerto puertaAeropuerto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.PuertaCode));

            puertaAeropuerto.Id = Crypt.Decryptar( consecutivo.Id);
            db.PuertaAeropuertoes.Add(PuertaAeropuertoCrypt.EncryptPuertaAeropuerto(puertaAeropuerto));
            db.Consecutivoes.Remove(consecutivo);
            db.SaveChanges();      
            return CreatedAtRoute("DefaultApi", new { puertaAeropuerto.Id }, puertaAeropuerto);
        }

        // DELETE: api/PuertaAeropuerto/5
        [ResponseType(typeof(PuertaAeropuerto))]
        public IHttpActionResult DeletePuertaAeropuerto(string id)
        {
            id = Crypt.Encryptar(id);
            var puertaAeropuerto = db.PuertaAeropuertoes.Find(id);
            if (puertaAeropuerto == null)
                return NotFound();
            db.PuertaAeropuertoes.Remove(puertaAeropuerto);
            db.SaveChanges();

            return Ok(PuertaAeropuertoCrypt.DecryptPuertaAeropuerto(puertaAeropuerto));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PuertaAeropuertoExists(string id)
        {
            return db.PuertaAeropuertoes.Count(e => e.Id == id) > 0;
        }
    }
}