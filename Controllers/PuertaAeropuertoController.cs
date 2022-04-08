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
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class PuertaAeropuertoController : ApiController
    {
        private VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();
        private readonly ConsecutivoRepository _consecutivo = new ConsecutivoRepository();

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
            {
                _error.SaveError("no se encontro la puerta de aeropuerto", "404");
                return NotFound();
            }
            return Ok(PuertaAeropuertoCrypt.DecryptPuertaAeropuerto(puertaAeropuerto));
        }

        // PUT: api/PuertaAeropuerto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPuertaAeropuerto(string id, PuertaAeropuerto puertaAeropuerto)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en puertas de aeropuerto", "400");
                return BadRequest(ModelState);
            }

            if (id != puertaAeropuerto.Id)
            {
                _error.SaveError("id's invalidas en puertas de aeropuerto", "400");
                return BadRequest();
            }

            puertaAeropuerto = PuertaAeropuertoCrypt.EncryptPuertaAeropuerto(puertaAeropuerto);
            db.Entry(puertaAeropuerto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertaAeropuertoExists(id))
                {
                    _error.SaveError("colision de id's en puertas de aeropuerto", "404");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _bitacora.SaveBitacora(puertaAeropuerto.Id, "modificar", "se modifico una puerta de aeropuerto", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PuertaAeropuerto
        [ResponseType(typeof(PuertaAeropuerto))]
        public async Task<IHttpActionResult> PostPuertaAeropuerto(PuertaAeropuerto puertaAeropuerto)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en puertas de aeropuerto", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.PuertaCode));
            if(consecutivo == null)
            {
                await _consecutivo.CreateConsecutivo("PA01", "Puertas del aeropuerto");
                consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.PuertaCode));
            }
            puertaAeropuerto.Id = Crypt.Decryptar( consecutivo.Id);
            db.PuertaAeropuertoes.Add(PuertaAeropuertoCrypt.EncryptPuertaAeropuerto(puertaAeropuerto));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertaAeropuertoExists(puertaAeropuerto.Id))
                {
                    _error.SaveError("colision de id's", "404");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _bitacora.SaveBitacora(puertaAeropuerto.Id, "crear", "se inserto una nueva puerta de aeropuerto", puertaAeropuerto.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
            return CreatedAtRoute("DefaultApi", new { puertaAeropuerto.Id }, puertaAeropuerto);
        }

        // DELETE: api/PuertaAeropuerto/5
        [ResponseType(typeof(PuertaAeropuerto))]
        public IHttpActionResult DeletePuertaAeropuerto(string id)
        {
            id = Crypt.Encryptar(id);
            var puertaAeropuerto = db.PuertaAeropuertoes.Find(id);
            if (puertaAeropuerto == null)
            {
                _error.SaveError("no se encontro la puerta", "404");
                return NotFound();
            }
            db.PuertaAeropuertoes.Remove(puertaAeropuerto);
            db.SaveChanges();

            _bitacora.SaveBitacora(id, "eliminar", "se elimino una puerta de aeropuerta", id);
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