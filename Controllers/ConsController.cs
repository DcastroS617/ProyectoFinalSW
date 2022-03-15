using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class ConsController : ApiController
    {
        private VVuelosEntity db = new VVuelosEntity();

        public List<Conscutivo2> Get()
        {
            return db.Conscutivo2.ToList();
        }

        public IHttpActionResult Post([FromBody] ConsecutivoTransaction consecutivo)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            for(int i = consecutivo.RangoInicial; i < consecutivo.RangoFinal + 1; i++)
            {
                db.Conscutivo2.Add(new Conscutivo2
                {
                    Id =  i.ToString(),
                    Prefijo = consecutivo.Prefijo,
                    Numero = i.ToString(),
                    Estado = consecutivo.Estado.ToString(),
                });
            }
            db.SaveChanges();
            return CreatedAtRoute(nameof(Get), new { consecutivo.RangoFinal }, consecutivo);
        }
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class ConsecutivoTransaction
    {
        public int RangoInicial { get; set; }
        public int RangoFinal { get; set; }
        public string Prefijo { get; set; }
        public bool Estado { get; set; }
    }
}
