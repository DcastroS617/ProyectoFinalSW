using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class PuertaAeropuertoCrypt
    {
        public static PuertaAeropuerto EncryptPuertaAeropuerto(PuertaAeropuerto puerta)
        {
            return new PuertaAeropuerto
            {
                Id = Crypt.Crypt.Encryptar(puerta.Id),
                Numero = Crypt.Crypt.Encryptar(puerta.Numero),
                Descripcion = Crypt.Crypt.Encryptar(puerta.Descripcion)
            };
        }
        public static PuertaAeropuerto DecryptPuertaAeropuerto(PuertaAeropuerto puerta)
        {
            return new PuertaAeropuerto
            {
                Id = Crypt.Crypt.Decryptar(puerta.Id),
                Numero = Crypt.Crypt.Decryptar(puerta.Numero),
                Descripcion = Crypt.Crypt.Decryptar(puerta.Descripcion)
            };
        }
        public static List<PuertaAeropuerto> DecryptPuertasAeropuerto(List<PuertaAeropuerto> puertas)
        {
            var returnList = new List<PuertaAeropuerto>();
            foreach(var puerta in puertas)
            {
                returnList.Add(DecryptPuertaAeropuerto(puerta));
            }
            return returnList;
        }
    }
}
/*
 return new PuertaAeropuerto
            {
                Id = Crypt.Crypt.Encryptar(Constants.AESKey, puerta.Id),
                Numero = Crypt.Crypt.Decryptar(Constants.AESKey, puerta.Numero),
                Descripcion = Crypt.Crypt.Decryptar(Constants.AESKey, puerta.Descripcion),
                PuertaActivas = puerta.PuertaActivas,
                Vueloes = puerta.Vueloes
            };
 */