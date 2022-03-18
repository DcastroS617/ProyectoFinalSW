
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

namespace ProyectoFinalSW.Controllers
{
    public class UserController : ApiController
    {
        private VVuelosEntities2 db = new VVuelosEntities2();

        // GET: api/User
        public List<User> GetUsers()
        {
            return UserCrypt.DecryptarUsers(db.Users.ToList());
        }

        // GET: api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            id = Crypt.Encryptar(id);
            var user = db.Users.Find(id);
            if (user == null)
                return NotFound();
            return Ok(UserCrypt.DecryptarUser(user));
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, User user)
        {
            //error handling
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return BadRequest();

            //action
            user = UserCrypt.EncryptarUser(user);
            db.Entry(user).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/User
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            db.Users.Add(UserCrypt.EncryptarUser(user));
            db.SaveChanges();           
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
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
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