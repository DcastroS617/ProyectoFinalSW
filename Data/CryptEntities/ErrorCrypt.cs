using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class ErrorCrypt
    {
        private static string Characters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";

        public static Error EncryptarError(Error error)
        {
            return new Error
            {
                Id = Crypt.Crypt.Encryptar(CreateId()),
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

        public static string CreateId()
        {
            var caracteres = Characters.Split(',');
            var repo = new ErrorRepository();
            var sb = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var randomCaracter = random.Next(0, (caracteres.Length - 1));
                sb.Append(caracteres[randomCaracter]);
            }
            if (repo.ValidateId(sb.ToString()))
            {
                CreateId();
            }
            return sb.ToString();
        }    
    }
}