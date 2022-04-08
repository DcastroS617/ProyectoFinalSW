using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoFinalSW.Repos
{
    public interface IErrorRepository : MainInterface
    {
        void SaveError(string mensaje, string numeroError);
        List<Error> GetAllErrors();
        Error GetError(string id);
    }
    public class ErrorRepository : IErrorRepository
    {
        private readonly VVuelosEntities _context = new VVuelosEntities();
        

        public List<Error> GetAllErrors()
        {
            return ErrorCrypt.DecryptarErrores(_context.Errors.ToList());
        }

        public Error GetError(string id)
        {
            id = Crypt.Encryptar(id);
            var error = _context.Errors.FirstOrDefault(e => e.Id.Equals(id));
            if (error == null) throw null;
            return ErrorCrypt.DecryptarError(error);
        }

        public void SaveError(string mensaje, string numeroError)
        {
            if(mensaje == null || numeroError == null) throw null;
            _context.Errors.Add(ErrorCrypt.EncryptarError(new Error
            {
                Id = "",
                Mensaje = mensaje,
                Fecha = DateTime.Now.ToString(),
                NumeroError = numeroError
            }));
            _context.SaveChanges();
        }
        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.Errors.FirstOrDefault(e => e.Id.Equals(id)) != null;
        }
    }
}