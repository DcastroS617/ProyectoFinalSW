using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class VueloController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        public List<Vuelo> GetVuelos()
        {
            return VueloCrypt.DecryptarVuelos(db.Vueloes.ToList());
        }
        public IHttpActionResult PostVuelo([FromBody] Vuelo vuelo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.VueloCode));
            vuelo.Id = Crypt.Decryptar(consecutivo.Id);
            db.Vueloes.Add(VueloCrypt.EncryptarVuelo(vuelo));
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { vuelo.Id }, vuelo);
        }     
        //crear joins que nos den un reflejo del vuelo y no los ID's de las entidades!!
    }
}
