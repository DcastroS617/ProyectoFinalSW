using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class AerolineaController : ApiController
    {
        private readonly VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();

        /// <summary>
        /// Controller que retorna una lista de todas las aerolineas guardadas en la Base de Datos
        /// </summary>
        /// <returns>Lista de aerolineas</returns>
        // GET: api/Aerolinea
        public List<Aerolinea> GetAerolineas()
        {
            return AerolineaCrypt.DecryptarAerolineas(db.Aerolineas.ToList());
        }
        /// <summary>
        /// Controller que se encarga de traer una aerolinea por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Una aerolinea</returns>
        // GET: api/Aerolinea/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult GetAerolinea(string id)
        {
            id = Crypt.Encryptar(id);
            var aerolinea = db.Aerolineas.FirstOrDefault(a => a.Id.Equals(id));
            if (aerolinea == null)
            {
                _error.SaveError("No se encuentra la aerolinea", "404");
                return NotFound();
            }
            return Ok(AerolineaCrypt.DecryptarAerolinea(aerolinea));
        }

        /// <summary>
        /// Controller que se encarga de modificar una aerolinea según su ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aerolinea"></param>
        /// <returns>La aerolinea modificada</returns>
        // PUT: api/Aerolinea/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAerolinea(string id, Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("Formulario Invalido en aerolineas", "400");
                return BadRequest(ModelState);
            }
            if (id != aerolinea.Id)
            {
                _error.SaveError("Id diferente en parametros", "400");
                return BadRequest();
            }
            aerolinea = AerolineaCrypt.EncryptarAerolinea(aerolinea);
            db.Entry(aerolinea).State = EntityState.Modified;
            try
            {             
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExists(id))
                    return NotFound();
                else
                    throw;
            }
            _bitacora.SaveBitacora(aerolinea.Id, "modificacion", "modificacion de aerolinea", aerolinea.Id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Controller que se encarga de encryptar y guardar una aerolinea en la Base de Datos
        /// </summary>
        /// <param name="aerolinea"></param>
        /// <returns>La aerolinea guardada</returns>
        // POST: api/Aerolinea
        [ResponseType(typeof(Aerolinea))]
        public async Task<IHttpActionResult> PostAerolinea(Aerolinea aerolinea)
        {
            var ConsecutivoRepo = new ConsecutivoRepository();
            if (!ModelState.IsValid)
            {
                _error.SaveError("Formulario Invalido en aerolineas", "400");
                return BadRequest(ModelState);
            }
            var consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.AerolineaCode));
            if (consecutivo == null)
            {
                await ConsecutivoRepo.CreateConsecutivo("AL01", "Aerolineas");
                consecutivo = db.Consecutivoes.FirstOrDefault(c => c.Entidad.Equals(Constants.AerolineaCode));
            }
            aerolinea.Id = Crypt.Decryptar(consecutivo.Id);
            db.Aerolineas.Add(AerolineaCrypt.EncryptarAerolinea(aerolinea));
            db.Consecutivoes.Remove(consecutivo);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExists(aerolinea.Id))
                {
                    _error.SaveError("colision de id's en aerolineas", "400");
                    return NotFound();
                }
            }      
            _bitacora.SaveBitacora(aerolinea.Id, "crear", "se inserto una nueva aerolinea", aerolinea.Id);
            _bitacora.SaveBitacora(consecutivo.Id, "eliminar", "se utilizo y deshecho un consecutivo", consecutivo.Id);
           
            return CreatedAtRoute("DefaultApi", new { aerolinea.Id }, aerolinea);
        }

        /// <summary>
        /// Controller que se encarga de eliminar una aerolinea por medio de su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>La aerolinea eliminada</returns>
        // DELETE: api/Aerolinea/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult DeleteAerolinea(string id)
        {
            id = Crypt.Encryptar(id);
            var aerolinea = db.Aerolineas.Find(id);
            var image = db.AerolineaImages.FirstOrDefault(i => i.AerolineaId == id);
            if (aerolinea == null || image == null)
            {
                _error.SaveError("No se encuentra la aerolinea", "404");
                return NotFound();
            }
            db.AerolineaImages.Remove(image);
            db.Aerolineas.Remove(aerolinea);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino una aerolinea", aerolinea.Id);
            return Ok(AerolineaCrypt.DecryptarAerolinea(aerolinea));
        }

        /// <summary>
        /// Controller que se encarga de subir la imagen que viene contenida en el
        /// Objeto http request (HttpContext)
        /// </summary>
        /// <returns>'Success' si se guardo la imagen</returns>
        [HttpPost]
        [Route("api/Aerolinea/UploadImage")]
        public async Task<string> UploadImage()
        {
            var context = HttpContext.Current;
            var root = context.Server.MapPath("~/Images/Aerolinea");
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
        /// Se encarga de realizar el proceso interno para guardar el binario de la imagen en la Base de Datos
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="fileName"></param>
        private void SaveFileBinarySQLServerEF(string localFile, string fileName)
        {

            byte[] fileBytes;
            var aerolineaID = db.Aerolineas.ToList();
            aerolineaID = AerolineaCrypt.DecryptarAerolineas(aerolineaID);
            using (var fs = new FileStream(localFile, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, Convert.ToInt32(fs.Length));
            }
            var file = new AerolineaImage
            {
                Id = "",
                ImageData = fileBytes,
                Name = fileName,
                Size = fileBytes.Length.ToString(),
                AerolineaId = aerolineaID[aerolineaID.Count - 1].Id,
            };
            file = AerolineaCrypt.EncryptarNewImage(file);
            db.AerolineaImages.Add(file);
            db.SaveChanges();
        }

        /// <summary>
        /// Controller utilizado para traer las imagenes de aerolineas almacenadas en la Base de Datos.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Imagen, logo de Aerolinea</returns>
        [HttpGet]
        [Route("api/Aerolinea/DownloadImage/{id}")]
        public HttpResponseMessage GetAerolineaImage(string id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var fileName = string.Empty;
            var fileBytes = new byte[0];
            id = Crypt.Encryptar(id);
            {
                var file = db.AerolineaImages.Where(image => image.AerolineaId == id).FirstOrDefault();
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AerolineaExists(string id)
        {
            return db.Aerolineas.Count(e => e.Id == id) > 0;
        }
    }
}