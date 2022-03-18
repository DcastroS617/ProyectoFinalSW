using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.CryptEntities
{
    public class UserCrypt
    {
        public static User EncryptarUser(User user)
        {
            return new ProyectoFinalSW.Models.User
            {
                Id = Crypt.Crypt.Encryptar(user.Id),
                Username = Crypt.Crypt.Encryptar(user.Username),
                Contrasena = Crypt.Crypt.Encryptar(user.Contrasena),
                Email = Crypt.Crypt.Encryptar(user.Email),               
                Role = Crypt.Crypt.Encryptar(user.Role)
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
                Role = Crypt.Crypt.Decryptar(user.Role)
            };
        }
        public static List<User> DecryptarUsers(List<User> users)
        {
            var returnList = new List<User>();
            foreach(var user in users)
            {
                returnList.Add(DecryptarUser(user));
            }
            return returnList;
        }
    }
}

/*
  return new ProyectoFinalSW.Models.User
            {
                Id = Crypt.Crypt.Encryptar(Constants.AESKey, user.Id),
                Username = Crypt.Crypt.Decryptar(Constants.AESKey, user.Username),
                Contrasena = Crypt.Crypt.Decryptar(Constants.AESKey, user.Contrasena),
                Email = Crypt.Crypt.Decryptar(Constants.AESKey, user.Email),
                Role = Crypt.Crypt.Decryptar(Constants.AESKey, user.Role),
                Compras = user.Compras,
                Reservas = user.Reservas
            };
 */