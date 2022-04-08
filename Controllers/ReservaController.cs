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
    public class ReservaController : ApiController
    {
        private VVuelosEntities db = new VVuelosEntities(); 
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();
        private readonly ConsecutivoRepository _consecutivo = new ConsecutivoRepository();

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
            {
                _error.SaveError("no se encontro la reserva", "404");
                return NotFound();
            }
            return Ok(ReservaCrypt.DecryptarReserva(reserva));
        }

        // PUT: api/Reserva/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReserva(string id, Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("el formulario es invalido en reservas", "400");
                return BadRequest(ModelState);
            }

            if (id != reserva.Id)
            {
                _error.SaveError("diferencia de id's en reservas", "400");
                return BadRequest();
            }

            reserva = ReservaCrypt.EncryptarReserva(reserva);
            db.Entry(reserva).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    _error.SaveError("colision de id's en reservas", "404");
                    return NotFound();
                }
                else
                    throw;
            }
            _bitacora.SaveBitacora(reserva.Id, "modificar", "se modifico una reserva", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reserva
        [ResponseType(typeof(Reserva))]
        public async Task<IHttpActionResult> PostReserva(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("el formulario es invalido en reservas", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(r => r.Entidad.Equals(Constants.ReservaCode));
            if(consecutivo == null)
            {
                await _consecutivo.CreateConsecutivo("RS01", "Reserva de boleto");
                consecutivo = db.Consecutivoes.FirstOrDefault(r => r.Entidad.Equals(Constants.ReservaCode));
            }
            reserva.Id = Crypt.Decryptar(consecutivo.Id);
            db.Reservas.Add(ReservaCrypt.EncryptarReserva(reserva));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(reserva.Id))
                {
                    _error.SaveError("colision de id's", "404");
                    return Conflict();
                }
                else
                    throw;
            }
            _bitacora.SaveBitacora(reserva.Id, "crear", "se inserto una nueva reserva", reserva.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
            return CreatedAtRoute("DefaultApi", new { id = reserva.Id }, reserva);
        }

        // DELETE: api/Reserva/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult DeleteReserva(string id)
        {
            id = Crypt.Encryptar(id);
            var reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                _error.SaveError("no se encontro la reserva", "404");
                return NotFound();
            }
            db.Reservas.Remove(reserva);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino una reserva", id);
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