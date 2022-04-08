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
    public class UserCrypt
    {
        private static string Characters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,1,2,3,4,5,6,7,8,9,0";
        public static User EncryptarUser(User user)
        {
            return new ProyectoFinalSW.Models.User
            {
                Id = Crypt.Crypt.Encryptar(user.Id),
                Username = Crypt.Crypt.Encryptar(user.Username),
                Contrasena = Crypt.Crypt.Encryptar(user.Contrasena),
                Email = Crypt.Crypt.Encryptar(user.Email),
                Role = Crypt.Crypt.Encryptar(user.Role),
                PreguntaSeguridad = Crypt.Crypt.Encryptar(user.PreguntaSeguridad),
                RespuestaSeguridad = Crypt.Crypt.Encryptar(user.RespuestaSeguridad)
            };
        }
        public static User EncryptarNewUser(User user)
        {
            return new ProyectoFinalSW.Models.User
            {
                Id = Crypt.Crypt.Encryptar(CreateId()),
                Username = Crypt.Crypt.Encryptar(user.Username),
                Contrasena = Crypt.Crypt.Encryptar(user.Contrasena),
                Email = Crypt.Crypt.Encryptar(user.Email),
                Role = Crypt.Crypt.Encryptar(user.Role),
                PreguntaSeguridad = Crypt.Crypt.Encryptar(user.PreguntaSeguridad),
                RespuestaSeguridad = Crypt.Crypt.Encryptar(user.RespuestaSeguridad)
            };
        }
            public static User DecryptarUser(User user)
        {
            return new ProyectoFinalSW.Models.User
            {
                Id = Crypt.Crypt.Decryptar(user.Id),
                Username = Crypt.Crypt.Decryptar(user.Username),
                Contrasena = Crypt.Crypt.Decryptar(user.Contrasena),
                Email = Crypt.Crypt.Decryptar(user.Email),
                Role = Crypt.Crypt.Decryptar(user.Role),
                PreguntaSeguridad = Crypt.Crypt.Decryptar(user.PreguntaSeguridad),
                RespuestaSeguridad = Crypt.Crypt.Decryptar(user.RespuestaSeguridad)
            };
        }
        public static List<User> DecryptarUsers(List<User> users)
        {
            var returnList = new List<User>();
            foreach (var user in users)
            {
                returnList.Add(DecryptarUser(user));
            }
            return returnList;
        }
        public static string CreateId()
        {
            var caracteres = Characters.Split(',');
            var repo = new UserRepository();
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
    public class Login
    {
        public string Username { get; set; }
        public string Contrasena { get; set; }

        public static Login EncryptarLogin(Login login)
        {
            return new Login
            {
                Username = Crypt.Crypt.Encryptar(login.Username),
                Contrasena = Crypt.Crypt.Encryptar(login.Contrasena)
            };
        }
    }
    
}

