using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class TarjetaCrypt
    {
        private static string Characters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
        public static Tarjeta EncryptarTarjeta(Tarjeta tarjeta)
        {
            return new Tarjeta
            {
                Id = Crypt.Crypt.Encryptar(tarjeta.Id),
                NumTarjeta = Crypt.Crypt.Encryptar(tarjeta.NumTarjeta),
                MesExp = Crypt.Crypt.Encryptar(tarjeta.MesExp),
                DiaExp = Crypt.Crypt.Encryptar(tarjeta.DiaExp),
                CVV = Crypt.Crypt.Encryptar(tarjeta.CVV),
                Monto = Crypt.Crypt.Encryptar(tarjeta.Monto),
                Tipo = Crypt.Crypt.Encryptar(tarjeta.Tipo),
                CompraId = Crypt.Crypt.Encryptar(tarjeta.CompraId)
            };
        }
        public static Tarjeta DecryptarTarjeta(Tarjeta tarjeta)
        {
            return new Tarjeta
            {
                Id = Crypt.Crypt.Decryptar(tarjeta.Id),
                NumTarjeta = Crypt.Crypt.Decryptar(tarjeta.NumTarjeta),
                MesExp = Crypt.Crypt.Decryptar(tarjeta.MesExp),
                DiaExp = Crypt.Crypt.Decryptar(tarjeta.DiaExp),
                CVV = Crypt.Crypt.Decryptar(tarjeta.CVV),
                Monto = Crypt.Crypt.Decryptar(tarjeta.Monto),
                Tipo = Crypt.Crypt.Decryptar(tarjeta.Tipo),
                CompraId = Crypt.Crypt.Decryptar(tarjeta.CompraId)
            };
        }
        public static List<Tarjeta> DecryptarTarjetas(List<Tarjeta> tarjetas)
        {
            var list = new List<Tarjeta>();
            foreach(var tarjeta in tarjetas)
            {
                list.Add(DecryptarTarjeta(tarjeta));
            }
            return list;
        }
        public static string CreateId()
        {
            var caracteres = Characters.Split(',');
            var repo = new TarjetaRepository();
            var sb = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var randomCaracter = random.Next(0, (caracteres.Length - 1));
                sb.Append(caracteres[randomCaracter]);
            }
            if (repo.ValidateId(sb.ToString()))
            {
                CreateId();
            }
            return sb.ToString();
        }
    }
}