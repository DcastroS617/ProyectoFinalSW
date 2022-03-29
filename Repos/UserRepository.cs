﻿using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using System.Linq;

namespace ProyectoFinalSW.Repos
{
    public class UserRepository : MainInterface
    {
        private readonly ProyectoFinalSW_dbEntities1 _context = new ProyectoFinalSW_dbEntities1();
        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.Users.FirstOrDefault(e => e.Id.Equals(id)) != null;
        }
        public bool Login(Login login)
        {
            var validar = _context.Users.FirstOrDefault(u => u.Username.Equals(login.Username) && u.Contrasena.Equals(login.Contrasena));
            return validar != null;
        }
    }
}