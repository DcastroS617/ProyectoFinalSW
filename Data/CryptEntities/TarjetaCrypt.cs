using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class TarjetaCrypt
    {
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
    }
}