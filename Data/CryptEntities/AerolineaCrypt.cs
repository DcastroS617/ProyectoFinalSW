using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class AerolineaCrypt
    {
        public static Aerolinea EncryptarAerolinea(Aerolinea aerolinea)
        {
            return new Aerolinea
            {
                Id = Crypt.Crypt.Encryptar(aerolinea.Id),
                Nombre = Crypt.Crypt.Encryptar(aerolinea.Nombre),
                Logo = Crypt.Crypt.Encryptar(aerolinea.Logo),
            };
        }
        public static Aerolinea DecryptarAerolinea(Aerolinea aerolinea)
        {
            return new Aerolinea
            {
                Id = Crypt.Crypt.Decryptar(aerolinea.Id),
                Nombre = Crypt.Crypt.Decryptar(aerolinea.Nombre),
                Logo = Crypt.Crypt.Decryptar(aerolinea.Logo),
            };
        }
        public static List<Aerolinea> DecryptarAerolineas(List<Aerolinea> aerolineas)
        {
            var returnLista = new List<Aerolinea>();
            foreach (var aero in aerolineas)
            {
                returnLista.Add(DecryptarAerolinea(aero));
            }
            return returnLista;
        }
    }
}

/*
  return new Aerolinea
            {
                Id = Crypt.Crypt.Encryptar(Constants.AESKey, aerolinea.Id),
                Nombre = Crypt.Crypt.Encryptar(Constants.AESKey, aerolinea.Nombre),
                Logo = Crypt.Crypt.Encryptar(Constants.AESKey, aerolinea.Logo),
                AerolineaPais = aerolinea.AerolineaPais,
                Vueloes = aerolinea.Vueloes
            };
 */