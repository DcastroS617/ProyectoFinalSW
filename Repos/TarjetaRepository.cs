using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Repos
{
    public class TarjetaRepository : MainInterface
    {
        private ProyectoFinalSW_dbEntities1 _context = new ProyectoFinalSW_dbEntities1();
        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.Tarjetas.FirstOrDefault(t => t.Id == id) != null;
        }
    }
}