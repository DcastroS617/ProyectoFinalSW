using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class CompraController : ApiController
    {
        private readonly VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();
        private readonly ConsecutivoRepository _consecutivo = new ConsecutivoRepository();

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
            {
                _error.SaveError("compra no encontrada", "404");
                return NotFound();
            }

            return Ok(CompraCrypt.DecryptarCompra(compra));
        }

        // PUT: api/Compra/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompra(string id, Compra compra)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en compras", "400");
                return BadRequest(ModelState);
            }

            if (id != compra.Id)
            {
                _error.SaveError("id diferente", "400");
                return BadRequest();
            }

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
                    _error.SaveError("colision de id's en compras", "404");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _bitacora.SaveBitacora(compra.Id, "modificacion", "modificacion de orden de compra", compra.Id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Compra
        [ResponseType(typeof(Compra))]
        public async Task<IHttpActionResult> PostCompra(Compra compra)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en compras", "400");
                return BadRequest(ModelState);
            }

            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.CompraCode));
            if(consecutivo == null)
            {
                await _consecutivo.CreateConsecutivo("CM01", "Compras de boletos");
                consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.CompraCode));
            }
            compra.Id = Crypt.Decryptar(consecutivo.Id);
            db.Compras.Add(CompraCrypt.EncryptarCompra(compra));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(compra.Id))
                {
                    _error.SaveError("colision de id's en compras", "409");
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            _bitacora.SaveBitacora(compra.Id, "crear", "se inserto una nueva compra", compra.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
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
                _error.SaveError("no se encontro la compra", "404");
                return NotFound();
            }

            db.Compras.Remove(compra);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino una compra", compra.Id);
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