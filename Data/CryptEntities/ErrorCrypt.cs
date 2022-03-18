using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class ErrorCrypt
    {
        public static Error EncryptarError(Error error)
        {
            return new Error
            {
                Id = Crypt.Crypt.Encryptar(error.Id),
                Mensaje = Crypt.Crypt.Encryptar(error.Mensaje),
                Fecha = Crypt.Crypt.Encryptar(error.Fecha),
                NumeroError = Crypt.Crypt.Encryptar(error.NumeroError),
            };
        }
        public static Error DecryptarError(Error error)
        {
            return new Error
            {
                Id = Crypt.Crypt.Decryptar(error.Id),
                Mensaje = Crypt.Crypt.Decryptar(error.Mensaje),
                Fecha = Crypt.Crypt.Decryptar(error.Fecha),
                NumeroError = Crypt.Crypt.Decryptar(error.NumeroError),
            };
        }
        public static List<Error> DecryptarErrores(List<Error> errores)
        {
            var list = new List<Error>();
            foreach (var error in errores)
            {
                list.Add(DecryptarError(error));
            }
            return list;
        }
    }
}