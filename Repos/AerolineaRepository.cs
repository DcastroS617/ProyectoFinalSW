using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Repos
{
    public class AerolineaRepository : MainInterface
    {
        private readonly VVuelosEntities _context = new VVuelosEntities();

        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.AerolineaImages.FirstOrDefault(a => a.Id == id) != null;
        }
    }
}