using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class OrigenCrypt
    {
        public static Origen EncryptarOrigen(Origen origen)
        {
            return new Origen
            {
                Id = Crypt.Crypt.Encryptar(origen.Id),
                Nombre = Crypt.Crypt.Encryptar(origen.Nombre),
                Descripcion = Crypt.Crypt.Encryptar(origen.Descripcion),
                Bandera = Crypt.Crypt.Encryptar(origen.Bandera)
            };
        }
        public static Origen DecryptarOrigen(Origen origen)
        {
            return new Origen
            {
                Id = Crypt.Crypt.Decryptar(origen.Id),
                Nombre = Crypt.Crypt.Decryptar(origen.Nombre),
                Descripcion = Crypt.Crypt.Decryptar(origen.Descripcion),
                Bandera = Crypt.Crypt.Decryptar(origen.Bandera)
            };
        }
        public static List<Origen> DecryptarOrigenes(List<Origen> origenes)
        {
            var listReturn = new List<Origen>();
            foreach (var origen in origenes)
            {
                listReturn.Add(DecryptarOrigen(origen));
            }
            return listReturn;
        }
    }
}

/*
  return new Origen
            {
                Id = Crypt.Crypt.Encryptar(Constants.AESKey, origen.Id),
                Nombre = Crypt.Crypt.Decryptar(Constants.AESKey, origen.Nombre),
                Descripcion = Crypt.Crypt.Decryptar(Constants.AESKey, origen.Descripcion),
                Bandera = Crypt.Crypt.Decryptar(Constants.AESKey, origen.Bandera),
                Vueloes = origen.Vueloes
            };
 */