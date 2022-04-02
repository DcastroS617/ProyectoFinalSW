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
        private readonly ProyectoFinalSW_dbEntities _context = new ProyectoFinalSW_dbEntities();
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
        private string CreatePrefijo()
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
    }
}