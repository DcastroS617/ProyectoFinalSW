
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using ProyectoFinalSW.Repos;

namespace ProyectoFinalSW.Controllers
{
    public class UserController : ApiController
    {
        private VVuelosEntities db = new VVuelosEntities();
        private readonly ErrorRepository _error = new ErrorRepository();
        private readonly BitacoraRepository _bitacora = new BitacoraRepository();

        // GET: api/User
        public List<User> GetUsers(string username = null)
        {
            var users = UserCrypt.DecryptarUsers(db.Users.ToList());
            if(username != null)
            {
                var result = users.Where(u => u.Username == username).ToList();
                return result;
            }
            return users;
        }

        // GET: api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            id = Crypt.Encryptar(id);
            var user = db.Users.Find(id);
            if (user == null)
            {
                _error.SaveError("no se encontro el usuario", "404");
                return NotFound();
            }
            return Ok(UserCrypt.DecryptarUser(user));
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            //error handling
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en usuarios", "400");
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                _error.SaveError("id's invalidos en usuarios", "400");
                return BadRequest();
            }

            //action
            user = UserCrypt.EncryptarUser(user);
            db.Entry(user).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(Crypt.Encryptar(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _bitacora.SaveBitacora(id, "modificar", "se modifico un usuario", id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/User
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                _error.SaveError("formulario invalido en usuarios", "400");
                return BadRequest(ModelState);
            }
            user = UserCrypt.EncryptarNewUser(user);
            db.Users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (UserExists(user.Id))
                {
                    _error.SaveError("Colision de id's en usuarios", "404");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }      
            _bitacora.SaveBitacora(user.Id, "insertar", "se inserto un nuevo usuario", user.Id);
            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            id = Crypt.Encryptar(id);
            var user = db.Users.Find(id);
            if (user == null)
            {
                _error.SaveError("no se encontro el usuario", "404");
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();
            _bitacora.SaveBitacora(id, "eliminar", "se elimino un usuario", id);
            return Ok(user);
        }

        [Route("api/user/login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody]Login login)
        {
            var repo = new UserRepository();
            login = Data.CryptEntities.Login.EncryptarLogin(login);
            var validar =  repo.Login(login);
            var user = db.Users.FirstOrDefault(u => u.Username == login.Username && u.Contrasena == login.Contrasena);
            if (!validar)
            {
                _error.SaveError("login invalido", "400");
                return BadRequest();
            }
            return Ok(UserCrypt.DecryptarUser(user));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}