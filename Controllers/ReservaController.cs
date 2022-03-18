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
    public class ReservaController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        // GET: api/Reserva
        public List<Reserva> GetReservas()
        {
            return ReservaCrypt.DecryptarReservas(db.Reservas.ToList());
        }

        // GET: api/Reserva/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult GetReserva(string id)
        {
            id = Crypt.Encryptar(id);
            var reserva = db.Reservas.FirstOrDefault(r => r.Id.Equals(id));
            if (reserva == null)
                return NotFound();
            return Ok(ReservaCrypt.DecryptarReserva(reserva));
        }

        // PUT: api/Reserva/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReserva(string id, Reserva reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != reserva.Id)
                return BadRequest();

            reserva = ReservaCrypt.EncryptarReserva(reserva);
            db.Entry(reserva).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reserva
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult PostReserva(Reserva reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var consecutivo = db.Consecutivoes.FirstOrDefault(r => r.Entidad.Equals(Constants.ReservaCode));
            reserva.Id = Crypt.Decryptar(consecutivo.Id);
            db.Reservas.Add(ReservaCrypt.EncryptarReserva(reserva));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReservaExists(reserva.Id))
                    return Conflict();
                else
                    throw;
            }
            return CreatedAtRoute("DefaultApi", new { id = reserva.Id }, reserva);
        }

        // DELETE: api/Reserva/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult DeleteReserva(string id)
        {
            id = Crypt.Encryptar(id);
            var reserva = db.Reservas.Find(id);
            if (reserva == null)
                return NotFound();
            db.Reservas.Remove(reserva);
            db.SaveChanges();
            return Ok(reserva);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservaExists(string id)
        {
            return db.Reservas.Count(e => e.Id == id) > 0;
        }
    }
}