using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class VueloController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        public List<JoinVuelo> GetVuelos([FromUri]string aerolineaQuery = null, [FromUri]string origenQuery = null)
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
                                  PuertaAeropuerto = puerta.Numero
                              }).ToList();
            joinVuelos = JoinVuelo.DecryptarVuelos(joinVuelos);

            if (aerolineaQuery != null)
                return joinVuelos.Where(v => v.Aerolinea.Contains(aerolineaQuery)).ToList();

            if (origenQuery != null)
                return joinVuelos.Where(v => v.Origen.Contains(origenQuery)).ToList();           

            return joinVuelos;
        }
        public IHttpActionResult PostVuelo([FromBody] Vuelo vuelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.VueloCode));
            vuelo.Id = Crypt.Decryptar(consecutivo.Id);
            db.Vueloes.Add(VueloCrypt.EncryptarVuelo(vuelo));
            db.Consecutivoes.Remove(consecutivo);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { vuelo.Id }, vuelo);
        }     
        //crear joins que nos den un reflejo del vuelo y no los ID's de las entidades!!

        public IHttpActionResult GetVuelo(string id)
        {
            id = Crypt.Encryptar(id);
            var vuelo = db.Vueloes.FirstOrDefault(v => v.Id.Equals(id));
            if (vuelo == null) return NotFound();
            return Ok(VueloCrypt.DecryptarVuelo(vuelo));
        }
        public IHttpActionResult PutVuelo(string id, Vuelo vuelo)
        {
            if(!ModelState.IsValid) return BadRequest();
            if(id != vuelo.Id) return BadRequest();

            vuelo = VueloCrypt.EncryptarVuelo(vuelo);
            db.Entry(vuelo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (VueloExists(vuelo.Id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult DeleteVuelo(string id)
        {
            id = Crypt.Encryptar(id);
            var vuelo = db.Vueloes.FirstOrDefault(v => v.Id.Equals(id));
            if(vuelo == null) return NotFound();
            db.Vueloes.Remove(vuelo);
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool VueloExists(string id) => db.Vueloes.Count(v => v.Id == id) > 0;
    }
}
