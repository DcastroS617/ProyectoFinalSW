using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class CompraCrypt
    {
        public static Compra EncryptarCompra(Compra compra)
        {
            return new Compra
            {
                Id = Crypt.Crypt.Encryptar(compra.Id),
                Descripcion = Crypt.Crypt.Encryptar(compra.Descripcion),
                VueloId = Crypt.Crypt.Encryptar(compra.VueloId),
                UserId = Crypt.Crypt.Encryptar(compra.UserId)
            };
        }
        public static Compra DecryptarCompra(Compra compra)
        {
            return new Compra
            {
                Id = Crypt.Crypt.Decryptar(compra.Id),
                Descripcion = Crypt.Crypt.Decryptar(compra.Descripcion),
                VueloId = Crypt.Crypt.Decryptar(compra.VueloId),
                UserId = Crypt.Crypt.Decryptar(compra.UserId)
            };
        }
        public static List<Compra> DecryptarCompras(List<Compra> compras)
        {
            var list = new List<Compra>();
            foreach (var compra in compras)
            {
                list.Add(DecryptarCompra(compra));  
            }
            return list;
        }
    }
}