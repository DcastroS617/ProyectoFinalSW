using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class ReservaCrypt
    {
        public static Reserva EncryptarReserva(Reserva reserva)
        {
            return new Reserva
            {
                Id = Crypt.Crypt.Encryptar(reserva.Id),
                Descripcion = Crypt.Crypt.Encryptar(reserva.Descripcion),
                UserId = Crypt.Crypt.Encryptar(reserva.UserId),
                VueloId = Crypt.Crypt.Encryptar(reserva.VueloId)
            };
        }
        public static Reserva DecryptarReserva(Reserva reserva)
        {
            return new Reserva
            {
                Id = Crypt.Crypt.Decryptar(reserva.Id),
                Descripcion = Crypt.Crypt.Decryptar(reserva.Descripcion),
                UserId = Crypt.Crypt.Decryptar(reserva.UserId),
                VueloId = Crypt.Crypt.Decryptar(reserva.VueloId)
            };
        }
        public static List<Reserva> DecryptarReservas(List<Reserva> reservas)
        {
            var list = new List<Reserva>();
            foreach (var compra in reservas)
            {
                list.Add(DecryptarReserva(compra));
            }
            return list;
        }
    }
}