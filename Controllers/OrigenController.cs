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
using Newtonsoft.Json;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class OrigenController : ApiController
    {
        private ProyectoFinalSW_dbEntities1 db = new ProyectoFinalSW_dbEntities1();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();

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
            {
                _error.SaveError("no se encontro el pais", "404");
                return NotFound();
            }
            return Ok(OrigenCrypt.DecryptarOrigen(origen));
        }

        // PUT: api/Origen/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrigen(string id, Origen origen)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("el formulario es invalido en paises", "400");
                return BadRequest(ModelState);
            }
            if (id != origen.Id)
            {
                _error.SaveError("id's diferentes en paises", "400");
                return BadRequest();
            }
            origen = OrigenCrypt.EncryptarOrigen(origen);
            db.Entry(origen).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrigenExists(id))
                {
                    _error.SaveError("colision de id's en paises", "404");
                    return NotFound();
                }
                else
                    throw;
            }
            _bitacora.SaveBitacora(origen.Id, "modificar", "se modifico un pais", origen.Id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Origen
        [ResponseType(typeof(Origen))]
        public IHttpActionResult PostOrigen(Origen origen)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en paises", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(o => o.Entidad.Equals(Constants.OrigenCode));
            origen.Id = Crypt.Decryptar(consecutivo.Id);
            db.Origens.Add(OrigenCrypt.EncryptarOrigen(origen));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrigenExists(origen.Id))
                {
                    _error.SaveError("colision de id's en paises", "404");
                    return NotFound();
                }
            }
            //return Ok(new { id = origen.Id });
            _bitacora.SaveBitacora(origen.Id, "crear", "se inserto un pais", origen.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
            return CreatedAtRoute("DefaultApi", new { origen.Id }, origen);
        }

        // DELETE: api/Origen/5
        [ResponseType(typeof(Origen))]
        public IHttpActionResult DeleteOrigen(string id)
        {
            id = Crypt.Encryptar(id);
            var origen = db.Origens.Find(id);
            if (origen == null)
            {
                _error.SaveError("no se encontro el pais", "404");
                return NotFound();
            }   
            db.Origens.Remove(origen);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino un pais", id);
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