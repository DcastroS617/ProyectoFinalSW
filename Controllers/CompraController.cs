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
    public class CompraController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        // GET: api/Compra
        public List<Compra> GetCompras()
        {
            return CompraCrypt.DecryptarCompras(db.Compras.ToList());
        }

        // GET: api/Compra/5
        [ResponseType(typeof(Compra))]
        public IHttpActionResult GetCompra(string id)
        {
            id = Crypt.Encryptar(id);
            var compra = db.Compras.Find(id);
            if (compra == null)
                return NotFound();

            return Ok(CompraCrypt.DecryptarCompra(compra));
        }

        // PUT: api/Compra/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompra(string id, Compra compra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != compra.Id)
                return BadRequest();

            compra = CompraCrypt.EncryptarCompra(compra);
            db.Entry(compra).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Compra
        [ResponseType(typeof(Compra))]
        public IHttpActionResult PostCompra(Compra compra)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.CompraCode));
            compra.Id = Crypt.Decryptar(consecutivo.Id);
            db.Compras.Add(CompraCrypt.EncryptarCompra(compra));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CompraExists(compra.Id))
                    return Conflict();
                else
                    throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = compra.Id }, compra);
        }

        // DELETE: api/Compra/5
        [ResponseType(typeof(Compra))]
        public IHttpActionResult DeleteCompra(string id)
        {
            id = Crypt.Encryptar(id);
            var compra = db.Compras.Find(id);
            if (compra == null)
            {
                return NotFound();
            }

            db.Compras.Remove(compra);
            db.SaveChanges();

            return Ok(compra);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompraExists(string id)
        {
            return db.Compras.Count(e => e.Id == id) > 0;
        }
    }
}