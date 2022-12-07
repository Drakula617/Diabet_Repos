using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiabetApp.Classes
{
    public class Md5Hesh
    {
        public static string HeshCode(string password)
        { 
            MD5 mD5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = mD5.ComputeHash(b);
            StringBuilder str = new StringBuilder();
            foreach (var a in hash)
            {
                str.Append(a.ToString("X2"));
            }
            return Convert.ToString(str.ToString());
        }
    }
}
