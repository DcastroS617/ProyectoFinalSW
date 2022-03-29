using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class BitacoraController : ApiController
    {
        private ProyectoFinalSW_dbEntities1 db = new ProyectoFinalSW_dbEntities1();
        public List<Bitacora> GetBitacoras() => BitacoraCrypt.DecryptarBitacoras(db.Bitacoras.ToList());
    }
}
