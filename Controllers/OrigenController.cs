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
    public class OrigenController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        // GET: api/Origen
        public List<Origen> GetOrigens()
        {         
            return OrigenCrypt.DecryptarOrigenes(db.Origens.ToList());
        }

        // GET: api/Origen/5
        [ResponseType(typeof(Origen))]
        public IHttpActionResult GetOrigen(string id)
        {
            id = Crypt.Encryptar(id);
            var origen = db.Origens.FirstOrDefault(o => o.Id.Equals(id));
            if (origen == null)
                return NotFound();
            return Ok(OrigenCrypt.DecryptarOrigen(origen));
        }

        // PUT: api/Origen/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrigen(string id, Origen origen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != origen.Id)
                return BadRequest();
            origen = OrigenCrypt.EncryptarOrigen(origen);
            db.Entry(origen).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrigenExists(id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Origen
        [ResponseType(typeof(Origen))]
        public IHttpActionResult PostOrigen(Origen origen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var consecutivo = db.Consecutivoes.FirstOrDefault(o => o.Entidad.Equals(Constants.OrigenCode));
            origen.Id = Crypt.Decryptar(consecutivo.Id);
            db.Origens.Add(OrigenCrypt.EncryptarOrigen(origen));
            db.Consecutivoes.Remove(consecutivo);
            db.SaveChanges();                      
            //return Ok(new { id = origen.Id });
            return CreatedAtRoute("DefaultApi", new { origen.Id }, origen);
        }

        // DELETE: api/Origen/5
        [ResponseType(typeof(Origen))]
        public IHttpActionResult DeleteOrigen(string id)
        {
            id = Crypt.Encryptar(id);
            var origen = db.Origens.Find(id);
            if (origen == null)            
                return NotFound();        
            db.Origens.Remove(origen);
            db.SaveChanges();
            return Ok(OrigenCrypt.DecryptarOrigen(origen));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrigenExists(string id)
        {
            return db.Origens.Count(e => e.Id == id) > 0;
        }
    }
}