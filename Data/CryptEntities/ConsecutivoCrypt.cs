﻿using ProyectoFinalSW.Data.Crypt;
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
                Descripcion = Crypt.Crypt.Encryptar(consecutivo.Descripcion),
                Entidad = Crypt.Crypt.Encryptar(consecutivo.Entidad)
            };
        }
        public static List<Consecutivo> EncryptarConsecutivos(List<Consecutivo> consecutivos)
        {
            var returnList = new List<Consecutivo>();
            foreach (var Con in consecutivos)
            {
                returnList.Add(EncryptarConsecutivo(Con));
            }
            return returnList;
        }
        public static Consecutivo DecryptarConsecutivo(Consecutivo consecutivo)
        {
            return new Consecutivo
            {
                Id = Crypt.Crypt.Decryptar(consecutivo.Id),
                Descripcion = Crypt.Crypt.Decryptar(consecutivo.Descripcion),
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