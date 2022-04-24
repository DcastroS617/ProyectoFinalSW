using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class OrigenCrypt
    {
        private static string Characters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
        public static Origen EncryptarOrigen(Origen origen)
        {
            return new Origen
            {
                Id = Crypt.Crypt.Encryptar(origen.Id),
                Nombre = Crypt.Crypt.Encryptar(origen.Nombre),
                Descripcion = Crypt.Crypt.Encryptar(origen.Descripcion),
            };
        }
        public static Origen DecryptarOrigen(Origen origen)
        {
            return new Origen
            {
                Id = Crypt.Crypt.Decryptar(origen.Id),
                Nombre = Crypt.Crypt.Decryptar(origen.Nombre),
                Descripcion = Crypt.Crypt.Decryptar(origen.Descripcion),
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

        public static OrigenImage EncryptarNewImage(OrigenImage image)
        {
            return new OrigenImage
            {
                Id = Crypt.Crypt.Encryptar(CreateId()),
                Name = Crypt.Crypt.Encryptar(image.Name),
                Size = Crypt.Crypt.Encryptar(image.Size),
                ImageData = image.ImageData,
                OrigenId = Crypt.Crypt.Encryptar(image.OrigenId),
            };
        }
        public static OrigenImage EncryptarImage(OrigenImage image)
        {
            return new OrigenImage
            {
                Id = Crypt.Crypt.Encryptar(image.Id),
                Name = Crypt.Crypt.Encryptar(image.Name),
                Size = Crypt.Crypt.Encryptar(image.Size),
                ImageData = image.ImageData,
                OrigenId = Crypt.Crypt.Encryptar(image.OrigenId),
            };
        }
        public static OrigenImage DecryptarImage(OrigenImage image)
        {
            return new OrigenImage
            {
                Id = Crypt.Crypt.Decryptar(image.Id),
                Name = Crypt.Crypt.Decryptar(image.Name),
                Size = Crypt.Crypt.Decryptar(image.Size),
                ImageData = image.ImageData,
                OrigenId = Crypt.Crypt.Decryptar(image.OrigenId),
            };
        }
        public static string CreateId()
        {
            var caracteres = Characters.Split(',');
            var repo = new OrigenRepository();
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