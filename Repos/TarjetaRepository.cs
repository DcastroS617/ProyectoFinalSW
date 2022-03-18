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
        VVuelosEntities2 _context = new VVuelosEntities2();
        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.Tarjetas.FirstOrDefault(t => t.Id == id) != null;
        }
    }
}