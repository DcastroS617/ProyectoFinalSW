using ProyectoFinalSW.Models;
using ProyectoFinalSW.Data.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class ConsecutivoCrypt
    {
        public static Consecutivo EncryptConsecutivo(Consecutivo consecutivo)
        {
            return new Consecutivo
            {
                Id = consecutivo.Id,
                Descripcion = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Descripcion),
                Prefijo = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.Prefijo),
                RangoInicial = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.RangoInicial),
                RangoFinal = Crypt.Crypt.Encryptar(Constants.AESKey, consecutivo.RangoFinal),
                Aerolineas = consecutivo.Aerolineas,
                Compras = consecutivo.Compras,
                Origens = consecutivo.Origens,
                PuertaAeropuertoes = consecutivo.PuertaAeropuertoes,
                Reservas = consecutivo.Reservas
            };
        }
        public static Consecutivo DecryptarConsecutivo(Consecutivo consecutivo)
        {
            return new Consecutivo
            {
                Id = consecutivo.Id,
                Descripcion = Crypt.Crypt.Decryptar(Constants.AESKey, consecutivo.Descripcion),
                Prefijo = Crypt.Crypt.Decryptar(Constants.AESKey, consecutivo.Prefijo),
                RangoInicial = Crypt.Crypt.Decryptar(Constants.AESKey, consecutivo.RangoInicial),
                RangoFinal = Crypt.Crypt.Decryptar(Constants.AESKey, consecutivo.RangoFinal),
                Aerolineas = consecutivo.Aerolineas,
                Compras = consecutivo.Compras,
                Origens = consecutivo.Origens,
                PuertaAeropuertoes = consecutivo.PuertaAeropuertoes,
                Reservas = consecutivo.Reservas
            };
        }
        public static List<Consecutivo> DecryptarConsecutivos(List<Consecutivo> consecutivos)
        {
            var listaReturn = new List<Consecutivo>();
            foreach (var con in consecutivos)
            {
                listaReturn.Add(DecryptarConsecutivo(con));
            }
            return listaReturn;
        }
    }
}