using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class VueloCrypt
    {
        public static Vuelo EncryptarVuelo(Vuelo vuelo)
        {
            return new Vuelo
            {
                Id = Crypt.Crypt.Encryptar(vuelo.Id),
                Descripcion = Crypt.Crypt.Encryptar(vuelo.Descripcion),
                AerolineaId = Crypt.Crypt.Encryptar(vuelo.AerolineaId),
                OrigenId = Crypt.Crypt.Encryptar(vuelo.OrigenId),
                PuertaAeropuertoId = Crypt.Crypt.Encryptar(vuelo.PuertaAeropuertoId)
            };
        }
        public static Vuelo DecryptarVuelo(Vuelo vuelo)
        {
            return new Vuelo
            {
                Id = Crypt.Crypt.Decryptar(vuelo.Id),
                Descripcion = Crypt.Crypt.Decryptar(vuelo.Descripcion),
                AerolineaId = Crypt.Crypt.Decryptar(vuelo.AerolineaId),
                OrigenId = Crypt.Crypt.Decryptar(vuelo.OrigenId),
                PuertaAeropuertoId = Crypt.Crypt.Decryptar(vuelo.PuertaAeropuertoId)
            };
        }
        public static List<Vuelo> DecryptarVuelos(List<Vuelo> vuelos)
        {
            var list = new List<Vuelo>();
            foreach (var vuelo in vuelos)
            {
                list.Add(DecryptarVuelo(vuelo));
            }
            return list;
        }
    }
}