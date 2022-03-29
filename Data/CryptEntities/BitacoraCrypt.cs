using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class BitacoraCrypt
    {
        private static string Characters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
        public static Bitacora EncryptarBitacora(Bitacora bitacora)
        {
            return new Bitacora
            {
                Id = Crypt.Crypt.Encryptar(CreateId()),
                Usuario = Crypt.Crypt.Encryptar(bitacora.Usuario),
                Fecha = Crypt.Crypt.Encryptar(bitacora.Fecha),
                Codigo = Crypt.Crypt.Encryptar(bitacora.Codigo),
                Tipo = Crypt.Crypt.Encryptar(bitacora.Tipo),
                Descripcion = Crypt.Crypt.Encryptar(bitacora.Descripcion),
                Dato = Crypt.Crypt.Encryptar(bitacora.Dato),
            };
        }
        public static Bitacora DecryptarBitacora(Bitacora bitacora)
        {
            return new Bitacora
            {
                Id = Crypt.Crypt.Decryptar(bitacora.Id),
                Usuario = Crypt.Crypt.Decryptar(bitacora.Usuario),
                Fecha = Crypt.Crypt.Decryptar(bitacora.Fecha),
                Codigo = Crypt.Crypt.Decryptar(bitacora.Codigo),
                Tipo = Crypt.Crypt.Decryptar(bitacora.Tipo),
                Descripcion = Crypt.Crypt.Decryptar(bitacora.Descripcion),
                Dato = Crypt.Crypt.Decryptar(bitacora.Dato)
            };
        }
        public static List<Bitacora> DecryptarBitacoras(List<Bitacora> bitacoras)
        {
            var list = new List<Bitacora>();
            foreach (var bitacora in bitacoras)
            {
                list.Add(DecryptarBitacora(bitacora));
            }
            return list;
        }
        public static string CreateId()
        {
            var caracteres = Characters.Split(',');
            var repo = new BitacoraRepository();
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