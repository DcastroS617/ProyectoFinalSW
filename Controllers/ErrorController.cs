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
        private ProyectoFinalSW_dbEntities1 db = new ProyectoFinalSW_dbEntities1();
        public List<Error> GetErrors() => ErrorCrypt.DecryptarErrores(db.Errors.ToList());
    }
}
