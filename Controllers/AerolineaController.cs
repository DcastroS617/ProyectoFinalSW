﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class AerolineaController : ApiController
    {
        private VVuelosEntity db = new VVuelosEntity();

        // GET: api/Aerolinea
        public List<Aerolinea> GetAerolineas()
        {
            return AerolineaCrypt.DecryptarAerolineas(db.Aerolineas.ToList());
        }

        // GET: api/Aerolinea/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult GetAerolinea(string id)
        {
            var aerolinea = db.Aerolineas.Find(id);
            if (aerolinea == null)
                return NotFound();
            return Ok(AerolineaCrypt.DecryptarAerolinea(aerolinea));
        }

        // PUT: api/Aerolinea/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAerolinea(string id, Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != aerolinea.Id)
                return BadRequest();
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
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Aerolinea
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult PostAerolinea(Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            db.Aerolineas.Add(AerolineaCrypt.EncryptarAerolinea(aerolinea));
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { aerolinea.Id }, aerolinea);
        }

        // DELETE: api/Aerolinea/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult DeleteAerolinea(string id)
        {
            Aerolinea aerolinea = db.Aerolineas.Find(id);
            if (aerolinea == null)
                return NotFound();
            db.Aerolineas.Remove(aerolinea);
            db.SaveChanges();
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