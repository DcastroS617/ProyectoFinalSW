using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProyectoFinalSW.Data.Crypt
{
    public class Crypt
    {
        public static string Encryptar(string sampleText)
        {
            return Convert.ToBase64String(new UnicodeEncoding().GetBytes(sampleText));
        }

        public static string Decryptar(string cypherText)
        {
            return new UnicodeEncoding().GetString(Convert.FromBase64String(cypherText));
        }
    }
}