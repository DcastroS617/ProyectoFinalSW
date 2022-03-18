using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Models;
using System.Linq;

namespace ProyectoFinalSW.Repos
{
    public class UserRepository : MainInterface
    {
        private readonly VVuelosEntities2 _context = new VVuelosEntities2();
        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.Users.FirstOrDefault(e => e.Id.Equals(id)) != null;
        }
    }
}