using ProyectoFinalSW.Data.Crypt;
using ProyectoFinalSW.Data.CryptEntities;
using ProyectoFinalSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinalSW.Repos
{
    public interface IBitacoraRepository : MainInterface
    {
        void SaveBitacora(string codigo, string tipo, string descripcion, string dato);
        Bitacora GetBitacora(string id);
        List<Bitacora> GetAllBitacoras();
    }
    public class BitacoraRepository : IBitacoraRepository
    {
        private readonly VVuelosEntities _context = new VVuelosEntities();

        public List<Bitacora> GetAllBitacoras()
        {
            return BitacoraCrypt.DecryptarBitacoras(_context.Bitacoras.ToList());
        }

        public Bitacora GetBitacora(string id)
        {
            id = Crypt.Encryptar(id);
            var bitacora = _context.Bitacoras.FirstOrDefault(b => b.Id.Equals(id));
            if (bitacora == null) throw null;
            return BitacoraCrypt.DecryptarBitacora(bitacora);
        }

        public void SaveBitacora(string codigo, string tipo, string descripcion, string dato)
        {
            var bitacora = new Bitacora
            {
                Id = "",
                Usuario = "test",
                Fecha = DateTime.Now.ToString(),
                Codigo = codigo,
                Tipo = tipo,
                Descripcion = descripcion,
                Dato = dato
            };
            _context.Bitacoras.Add(BitacoraCrypt.EncryptarBitacora(bitacora));
            _context.SaveChanges();
        }

        public bool ValidateId(string id)
        {
            id = Crypt.Encryptar(id);
            return _context.Bitacoras.FirstOrDefault(b => b.Id.Equals(id)) != null;
        }
    }
}