using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class VueloController : ApiController
    {
        private readonly VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();
        private readonly ConsecutivoRepository _consecutivo = new ConsecutivoRepository();

        public List<JoinVuelo> GetVuelos([FromUri]string Aerolinea = null, [FromUri]string Origen = null)
        {
            //join para dar la visualizacion real de la tabla de vuelos disponibles
            var joinVuelos = (from vuelo in db.Vueloes 
                              join aerolinea in db.Aerolineas
                              on vuelo.AerolineaId equals aerolinea.Id
                              join origen in db.Origens
                              on vuelo.OrigenId equals origen.Id
                              join puerta in db.PuertaAeropuertoes
                              on vuelo.PuertaAeropuertoId equals puerta.Id
                              select new JoinVuelo
                              {
                                  Id = vuelo.Id,
                                  Descripcion = vuelo.Descripcion,
                                  Aerolinea = aerolinea.Nombre,
                                  Origen = origen.Nombre,
                                  Provincia = origen.Descripcion,
                                  PuertaAeropuerto = puerta.Numero
                              }).ToList();
            joinVuelos = JoinVuelo.DecryptarVuelos(joinVuelos);

            if (Aerolinea != null)
                return joinVuelos.Where(v => v.Aerolinea.Contains(Aerolinea.ToLower())).ToList();

            if (Origen != null)
                return joinVuelos.Where(v => v.Origen.Contains(Origen.ToLower())).ToList();           

            return joinVuelos;
        }
        public async Task<IHttpActionResult> PostVuelo([FromBody] Vuelo vuelo)
        {
            if (!ModelState.IsValid) 
            {
                _error.SaveError("formulario invalido en vuelos", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.VueloCode));
            if(consecutivo == null)
            {
                await _consecutivo.CreateConsecutivo("VL01", "Vuelos disponibles");
                consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.VueloCode));
            }
            vuelo.Id = Crypt.Decryptar(consecutivo.Id);
            db.Vueloes.Add(VueloCrypt.EncryptarVuelo(vuelo));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VueloExists(vuelo.Id))
                {
                    _error.SaveError("colision de id's en vuelos", "409");
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            _bitacora.SaveBitacora(vuelo.Id, "crear", "se inserto un nuevo vuelo", vuelo.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho el consecutivo", consecutivo.Id);
            return CreatedAtRoute("DefaultApi", new { vuelo.Id }, vuelo);
        }

        public IHttpActionResult GetVuelo(string id)
        {
            id = Crypt.Encryptar(id);
            var vuelo = db.Vueloes.FirstOrDefault(v => v.Id.Equals(id));
            if (vuelo == null) 
            {
                _error.SaveError("no se encontro el vuelo", "404");
                return NotFound();
            }
            return Ok(VueloCrypt.DecryptarVuelo(vuelo));
        }
        public IHttpActionResult PutVuelo(string id, Vuelo vuelo)
        {
            if(!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en vuelos", "400");
                return BadRequest();
            }
            if(id != vuelo.Id)
            {
                _error.SaveError("id's diferentes en vuelos", "400");
                return BadRequest();
            }

            vuelo = VueloCrypt.EncryptarVuelo(vuelo);
            db.Entry(vuelo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VueloExists(vuelo.Id))
                {
                    _error.SaveError("colision de id's vuelos", "409");
                    return Conflict();
                }
                else
                    throw;
            }
            _bitacora.SaveBitacora(id, "modificar", "se modifico un vuelo", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult DeleteVuelo(string id)
        {
            id = Crypt.Encryptar(id);
            var vuelo = db.Vueloes.FirstOrDefault(v => v.Id.Equals(id));
            if(vuelo == null) 
            {
                _error.SaveError("no se encontro el vuelo", "404");
                return NotFound();
            }
            db.Vueloes.Remove(vuelo);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino un vuelo", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool VueloExists(string id) => db.Vueloes.Count(v => v.Id == id) > 0;
    }
}
