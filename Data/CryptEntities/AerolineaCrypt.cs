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
    public class AerolineaCrypt
    {
        private static string Characters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
        public static Aerolinea EncryptarAerolinea(Aerolinea aerolinea)
        {
            return new Aerolinea
            {
                Id = Crypt.Crypt.Encryptar(aerolinea.Id),
                Nombre = Crypt.Crypt.Encryptar(aerolinea.Nombre),
                Origen = Crypt.Crypt.Encryptar(aerolinea.Origen),
            };
        }
        public static Aerolinea DecryptarAerolinea(Aerolinea aerolinea)
        {
            return new Aerolinea
            {
                Id = Crypt.Crypt.Decryptar(aerolinea.Id),
                Nombre = Crypt.Crypt.Decryptar(aerolinea.Nombre),
                Origen = Crypt.Crypt.Decryptar(aerolinea.Origen),
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
        public static AerolineaImage EncryptarNewImage(AerolineaImage image)
        {
            return new AerolineaImage
            {
                Id = Crypt.Crypt.Encryptar(CreateId()),
                Name = Crypt.Crypt.Encryptar(image.Name),
                Size = Crypt.Crypt.Encryptar(image.Size),
                ImageData = image.ImageData,
                AerolineaId = Crypt.Crypt.Encryptar(image.AerolineaId),
            };
        }
        public static AerolineaImage EncryptarImage(AerolineaImage image)
        {
            return new AerolineaImage
            {
                Id = Crypt.Crypt.Encryptar(image.Id),
                Name = Crypt.Crypt.Encryptar(image.Name),
                Size = Crypt.Crypt.Encryptar(image.Size),
                ImageData = image.ImageData,
                AerolineaId = Crypt.Crypt.Encryptar(image.AerolineaId),
            };
        }
        public static AerolineaImage DecryptarImage(AerolineaImage image)
        {
            return new AerolineaImage
            {
                Id = Crypt.Crypt.Decryptar(image.Id),
                Name = Crypt.Crypt.Decryptar(image.Name),
                Size = Crypt.Crypt.Decryptar(image.Size),
                ImageData = image.ImageData,
                AerolineaId = Crypt.Crypt.Decryptar(image.AerolineaId),
            };
        }
        public static string CreateId()
        {
            var caracteres = Characters.Split(',');
            var repo = new AerolineaRepository();
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
  return new Aerolinea
            {
                Id = Crypt.Crypt.Encryptar(Constants.AESKey, aerolinea.Id),
                Nombre = Crypt.Crypt.Encryptar(Constants.AESKey, aerolinea.Nombre),
                Logo = Crypt.Crypt.Encryptar(Constants.AESKey, aerolinea.Logo),
                AerolineaPais = aerolinea.AerolineaPais,
                Vueloes = aerolinea.Vueloes
            };
 */