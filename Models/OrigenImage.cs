//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoFinalSW.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrigenImage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public byte[] ImageData { get; set; }
        public string OrigenId { get; set; }
    
        public virtual Origen Origen { get; set; }
    }
}
