using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
    public class AerolineaController : ApiController
    {
        private readonly ProyectoFinalSW_dbEntities db = new ProyectoFinalSW_dbEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();

        // GET: api/Aerolinea
        public List<Aerolinea> GetAerolineas()
        {
            return AerolineaCrypt.DecryptarAerolineas(db.Aerolineas.ToList());
        }

        // GET: api/Aerolinea/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult GetAerolinea(string id)
        {
            id = Crypt.Encryptar(id);
            var aerolinea = db.Aerolineas.FirstOrDefault(a => a.Id.Equals(id));
            if (aerolinea == null)
            {
                _error.SaveError("No se encuentra la aerolinea", "404");
                return NotFound();
            }
            return Ok(AerolineaCrypt.DecryptarAerolinea(aerolinea));
        }

        // PUT: api/Aerolinea/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAerolinea(string id, Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("Formulario Invalido en aerolineas", "400");
                return BadRequest(ModelState);
            }
            if (id != aerolinea.Id)
            {
                _error.SaveError("Id diferente en parametros", "400");
                return BadRequest();
            }
            aerolinea = AerolineaCrypt.EncryptarAerolinea(aerolinea);
            db.Entry(aerolinea).State = EntityState.Modified;
            try
            {             
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExists(id))
                    return NotFound();
                else
                    throw;
            }
            _bitacora.SaveBitacora(aerolinea.Id, "modificacion", "modificacion de aerolinea", aerolinea.Id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Aerolinea
        [ResponseType(typeof(Aerolinea))]
        public async Task<IHttpActionResult> PostAerolinea(Aerolinea aerolinea)
        {
            var ConsecutivoRepo = new ConsecutivoRepository();
            if (!ModelState.IsValid)
            {
                _error.SaveError("Formulario Invalido en aerolineas", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.AerolineaCode));
            if (consecutivo == null)
            {
                await ConsecutivoRepo.CreateConsecutivo("AL01", "Aerolineas");
                consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.AerolineaCode));
            }
            aerolinea.Id = Crypt.Decryptar(consecutivo.Id);
            db.Aerolineas.Add(AerolineaCrypt.EncryptarAerolinea(aerolinea));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExists(aerolinea.Id))
                {
                    _error.SaveError("colision de id's en aerolineas", "400");
                    return NotFound();
                }
            }      
            _bitacora.SaveBitacora(aerolinea.Id, "crear", "se inserto una nueva aerolinea", aerolinea.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
           
            return CreatedAtRoute("DefaultApi", new { aerolinea.Id }, aerolinea);
        }

        // DELETE: api/Aerolinea/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult DeleteAerolinea(string id)
        {
            id = Crypt.Encryptar(id);
            var aerolinea = db.Aerolineas.Find(id);
            if (aerolinea == null)
            {
                _error.SaveError("No se encuentra la aerolinea", "404");
                return NotFound();
            }
            db.Aerolineas.Remove(aerolinea);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino una aerolinea", aerolinea.Id);
            return Ok(AerolineaCrypt.DecryptarAerolinea(aerolinea));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AerolineaExists(string id)
        {
            return db.Aerolineas.Count(e => e.Id == id) > 0;
        }
    }
}