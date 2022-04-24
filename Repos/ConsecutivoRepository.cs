using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProyectoFinalSW.Repos
{
    public class ConsecutivoRepository : MainInterface
    {
        private readonly VVuelosEntities _context = new VVuelosEntities();
        private readonly string Caracteres = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public bool ValidateId(string id)
        {
            var list = ConsecutivoCrypt.DecryptarConsecutivos(_context.Consecutivoes.ToList());
            return list.FirstOrDefault(c => c.Id.StartsWith(id)) != null;
        }
        public async Task CreateConsecutivo(string entidad, string descripcion, int rangoInicial = 100, int rangoFinal = 105, string prefijo = null)
        {
            if(prefijo == null)
            {
                prefijo = CreatePrefijo();
            }
            for (int i = rangoInicial; i < rangoFinal + 1; i++)
            {
                _context.Consecutivoes.Add(ConsecutivoCrypt.EncryptarConsecutivo(new Consecutivo
                {
                    Id = prefijo + i.ToString(),
                    Descripcion = descripcion,
                    Entidad = entidad
                }));
            }
            await _context.SaveChangesAsync();
        }
        public string CreatePrefijo()
        {
            var chars = Caracteres.Split(',');
            var Random = new Random();
            var sb = new StringBuilder();
            for(int i = 0; i < 2; i++)
            {
                var randomNum = Random.Next(0, chars.Length - 1);
                sb.Append(chars[randomNum]);
            }
            if (ValidateId(sb.ToString()))
            {
                CreatePrefijo();
            }
            return sb.ToString();
        }

        public List<ConsecutivoAnalytics> GetConsecutivoAnalytics()
        {
            var response = new List<ConsecutivoAnalytics>();

            var aerolineas = (from a in _context.Consecutivoes where a.Entidad == Constants.AerolineaCode select a).ToList();           
            if (aerolineas.Any()) 
            {
                aerolineas = ConsecutivoCrypt.DecryptarConsecutivos(aerolineas);
                response.Add(new ConsecutivoAnalytics { Cantidad = aerolineas.Count, Entidad = "Aerolineas", Ejemplar = aerolineas.FirstOrDefault()?.Id });
            }         

            var compras = (from c in _context.Consecutivoes where c.Entidad == Constants.CompraCode select c).ToList();
            if (compras.Any())
            {
                compras = ConsecutivoCrypt.DecryptarConsecutivos(compras);
                response.Add(new ConsecutivoAnalytics { Cantidad = compras.Count, Entidad = "Compra de boletos", Ejemplar = compras.FirstOrDefault()?.Id });
            }
            
            var destinos = (from d in _context.Consecutivoes where d.Entidad == Constants.OrigenCode select d).ToList();
            if (destinos.Any())
            {
                destinos = ConsecutivoCrypt.DecryptarConsecutivos(destinos);
                response.Add(new ConsecutivoAnalytics { Cantidad = destinos.Count, Entidad = "Paises de destino", Ejemplar = destinos.FirstOrDefault()?.Id });
            }

            var puertas = (from p in _context.Consecutivoes where p.Entidad == Constants.PuertaCode select p).ToList();
            if (puertas.Any())
            {
                puertas = ConsecutivoCrypt.DecryptarConsecutivos(puertas);
                response.Add(new ConsecutivoAnalytics { Cantidad = puertas.Count, Entidad = "Puertas del aeropuerto", Ejemplar = puertas.FirstOrDefault()?.Id });
            }

            var reservas = (from r in _context.Consecutivoes where r.Entidad == Constants.ReservaCode select r).ToList();
            if (reservas.Any())
            {
                reservas = ConsecutivoCrypt.DecryptarConsecutivos(reservas);        
                response.Add(new ConsecutivoAnalytics { Cantidad = reservas.Count, Entidad = "reserva de boletos", Ejemplar = reservas.FirstOrDefault()?.Id });
            }

            var vuelos = (from v in _context.Consecutivoes where v.Entidad == Constants.VueloCode select v).ToList();
            if (vuelos.Any())
            {
                vuelos = ConsecutivoCrypt.DecryptarConsecutivos(vuelos);
                response.Add(new ConsecutivoAnalytics { Cantidad = vuelos.Count, Entidad = "Vuelos disponibles", Ejemplar = vuelos.FirstOrDefault()?.Id });
            }

            return response;
        }
    }
    public class ConsecutivoAnalytics
    {
        public string Entidad { get; set; }
        public int Cantidad { get; set; }
        public string Ejemplar { get; set; }
    }
}