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
        private ProyectoFinalSW_dbEntities db = new ProyectoFinalSW_dbEntities();
        public List<Error> GetErrors() => ErrorCrypt.DecryptarErrores(db.Errors.ToList());
    }
}
