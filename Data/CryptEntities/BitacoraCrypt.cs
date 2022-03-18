using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class BitacoraCrypt
    {
        public static Bitacora EncryptarBitacora(Bitacora bitacora)
        {
            return new Bitacora
            {
                Id = Crypt.Crypt.Encryptar(bitacora.Id),
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
                Dato = Crypt.Crypt.Decryptar(bitacora.Dato),
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
    }
}