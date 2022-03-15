using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Data.Crypt
{
    public class UserCrypt
    {
        public static User EncryptarUser(User user)
        {
            return new ProyectoFinalSW.Models.User
            {
                Id = user.Id,
                Username = Crypt.Encryptar(Constants.AESKey,user.Username),
                Contrasena = Crypt.Encryptar(Constants.AESKey, user.Contrasena),
                Email = Crypt.Encryptar(Constants.AESKey, user.Email),               
                Role = Crypt.Encryptar(Constants.AESKey, user.Role),
                Compras = user.Compras,
                Reservas = user.Reservas
            };
        }
        public static User DecryptarUser(User user)
        {
            return new ProyectoFinalSW.Models.User
            {
                Id = user.Id,
                Username = Crypt.Decryptar(Constants.AESKey, user.Username),
                Contrasena = Crypt.Decryptar(Constants.AESKey, user.Contrasena),
                Email = Crypt.Decryptar(Constants.AESKey, user.Email),
                Role = Crypt.Decryptar(Constants.AESKey, user.Role),
                Compras = user.Compras,
                Reservas = user.Reservas
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