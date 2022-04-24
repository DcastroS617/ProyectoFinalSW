using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class OrigenController : ApiController
    {
        private VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();
        private readonly ConsecutivoRepository _consecutivo = new ConsecutivoRepository();

        /// <summary>
        /// Controller que retorna una lista de todos los paises de destino guardados en la Base de Datos
        /// </summary>
        /// <returns>Lista de paises destino</returns>
        // GET: api/Origen
        public List<Origen> GetOrigens()
        {         
            return OrigenCrypt.DecryptarOrigenes(db.Origens.ToList());
        }

        /// <summary>
        /// Controller que se encarga de traer un pais de destino por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un pais destino</returns>
        // GET: api/Origen/5
        [ResponseType(typeof(Origen))]
        public IHttpActionResult GetOrigen(string id)
        {
            id = Crypt.Encryptar(id);
            var origen = db.Origens.FirstOrDefault(o => o.Id.Equals(id));
            if (origen == null)
            {
                _error.SaveError("no se encontro el pais", "404");
                return NotFound();
            }
            return Ok(OrigenCrypt.DecryptarOrigen(origen));
        }

        /// <summary>
        /// Controller que se encarga de modificar un pais de destino según su ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="origen"></param>
        /// <returns>El destino modificado</returns>
        // PUT: api/Origen/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrigen(string id, Origen origen)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("el formulario es invalido en paises", "400");
                return BadRequest(ModelState);
            }
            if (id != origen.Id)
            {
                _error.SaveError("id's diferentes en paises", "400");
                return BadRequest();
            }
            origen = OrigenCrypt.EncryptarOrigen(origen);
            db.Entry(origen).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrigenExists(id))
                {
                    _error.SaveError("colision de id's en paises", "404");
                    return NotFound();
                }
                else
                    throw;
            }
            _bitacora.SaveBitacora(origen.Id, "modificar", "se modifico un pais", origen.Id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Controller que se encarga de encryptar y guardar un pais de destino en la Base de Datos
        /// </summary>
        /// <param name="origen"></param>
        /// <returns>El destino guardado</returns>
        // POST: api/Origen
        [ResponseType(typeof(Origen))]
        public async Task<IHttpActionResult> PostOrigen(Origen origen)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en paises", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(o => o.Entidad.Equals(Constants.OrigenCode));
            if(consecutivo == null)
            {
                await _consecutivo.CreateConsecutivo("OR01", "Pais destino");
                consecutivo = db.Consecutivoes.FirstOrDefault(o => o.Entidad.Equals(Constants.OrigenCode));
            }
            origen.Id = Crypt.Decryptar(consecutivo.Id);
            db.Origens.Add(OrigenCrypt.EncryptarOrigen(origen));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrigenExists(origen.Id))
                {
                    _error.SaveError("colision de id's en paises", "404");
                    return NotFound();
                }
            }
            //return Ok(new { id = origen.Id });
            _bitacora.SaveBitacora(origen.Id, "crear", "se inserto un pais", origen.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
            return CreatedAtRoute("DefaultApi", new { origen.Id }, origen);
        }

        /// <summary>
        /// Controller que se encarga de eliminar un pais destino por medio de su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>El destino eliminado</returns>
        // DELETE: api/Origen/5
        [ResponseType(typeof(Origen))]
        public IHttpActionResult DeleteOrigen(string id)
        {
            id = Crypt.Encryptar(id);
            var origen = db.Origens.Find(id);
            var image = db.OrigenImages.FirstOrDefault(i => i.OrigenId == id);
            if (origen == null || image == null)
            {
                _error.SaveError("no se encontro el pais", "404");
                return NotFound();
            }   
            db.OrigenImages.Remove(image);
            db.Origens.Remove(origen);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino un pais", id);
            return Ok(OrigenCrypt.DecryptarOrigen(origen));
        }

        /// <summary>
        /// Controller que se encarga de subir la imagen que viene contenida en el
        /// Objeto http request (HttpContext)
        /// </summary>
        /// <returns>'Success' si se guardo la imagen</returns>
        [HttpPost]
        [Route("api/Origen/UploadImage")]
        public async Task<string> UploadImage()
        {
            var context = HttpContext.Current;
            var root = context.Server.MapPath("~/Images/Bandera");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.FileData)
                {
                    var name = file.Headers.ContentDisposition.FileName;
                    name = name.Trim('"');
                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(root, name);
                    SaveFileBinarySQLServerEF(localFileName, filePath);
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                return "Success";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Controller utilizado para traer las imagenes de origenes almacenadas en la Base de Datos.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Imagen, Pais destino</returns>
        [HttpGet]
        [Route("api/Origen/DownloadImage/{id}")]
        public HttpResponseMessage GetOrigenImage(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var fileName = string.Empty;
            var fileBytes = new byte[0];
            id = Crypt.Encryptar(id);
            {
                var file = db.OrigenImages.Where(image => image.OrigenId.Contains(id)).FirstOrDefault();
                if (file != null)
                {
                    fileName = file.Name;
                    fileBytes = file.ImageData;
                }
            }
            if (fileBytes.Length == 0)
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                var fileMemStream = new MemoryStream(fileBytes);
                result.Content = new StreamContent(fileMemStream);
                var headers = result.Content.Headers;
                headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                headers.ContentDisposition.FileName = fileName;
                headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                headers.ContentLength = fileMemStream.Length;
            }
            return result;
        }

        /// <summary>
        /// Se encarga de realizar el proceso interno para guardar el binario de la imagen en la Base de Datos
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="fileName"></param>
        private void SaveFileBinarySQLServerEF(string localFile, string fileName)
        {

            byte[] fileBytes;
            var origenID = db.Origens.ToList();
            origenID = OrigenCrypt.DecryptarOrigenes(origenID);
            using (var fs = new FileStream(localFile, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, Convert.ToInt32(fs.Length));
            }
            var file = new OrigenImage
            {
                Id = "",
                ImageData = fileBytes,
                Name = fileName,
                Size = fileBytes.Length.ToString(),
                OrigenId = origenID[origenID.Count - 1].Id,
            };
            file = OrigenCrypt.EncryptarNewImage(file);
            db.OrigenImages.Add(file);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrigenExists(string id)
        {
            return db.Origens.Count(e => e.Id == id) > 0;
        }
    }
}