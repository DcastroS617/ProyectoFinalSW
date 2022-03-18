using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class ConsecutivoCrypt
    {
        public static Consecutivo EncryptarConsecutivo(Consecutivo consecutivo)
        {
            return new Consecutivo
            {
                Id = Crypt.Crypt.Encryptar(consecutivo.Id),
                Prefijo = Crypt.Crypt.Encryptar(consecutivo.Prefijo),
                Numero = Crypt.Crypt.Encryptar(consecutivo.Numero),
                Estado = Crypt.Crypt.Encryptar(consecutivo.Estado),
                Entidad = Crypt.Crypt.Encryptar(consecutivo.Entidad)
            };
        }       
        public static Consecutivo DecryptarConsecutivo(Consecutivo consecutivo)
        {
            return new Consecutivo
            {
                Id = Crypt.Crypt.Decryptar(consecutivo.Id),
                Prefijo = Crypt.Crypt.Decryptar(consecutivo.Prefijo),
                Numero = Crypt.Crypt.Decryptar(consecutivo.Numero),
                Estado = Crypt.Crypt.Decryptar(consecutivo.Estado),
                Entidad = Crypt.Crypt.Decryptar(consecutivo.Entidad)
            };
        }
        public static List<Consecutivo> DecryptarConsecutivos(List<Consecutivo> consecutivos)
        {
            var returnList = new List<Consecutivo>();
            foreach (var Con in consecutivos)
            {
                returnList.Add(DecryptarConsecutivo(Con));
            }
            return returnList;
        }
    }
}
/*
 return new Consecutivo
            {
                Id = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Id),
                Prefijo = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Prefijo),
                Numero = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Numero),
                Estado = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Estado),
                Entidad = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Entidad)
            };
 */