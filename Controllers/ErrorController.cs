using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProyectoFinalSW.Controllers
{
    public class ErrorController : ApiController
    {
        private VVuelosEntities db = new VVuelosEntities();
        public List<Error> GetErrors(string fecha = null) 
        {
            var errors = ErrorCrypt.DecryptarErrores(db.Errors.ToList());
            if (fecha != null)
            {
                var result = errors.Where(e => e.Fecha.Contains(fecha)).ToList();
                return result;
            }
            return errors;
        }

    }
}
