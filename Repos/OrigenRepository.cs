using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Repos
{
    public class OrigenRepository : MainInterface
    {
        public VVuelosEntities db = new VVuelosEntities();
        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return db.OrigenImages.FirstOrDefault(x => x.Id.Equals(id)) != null;
        }
    }
}