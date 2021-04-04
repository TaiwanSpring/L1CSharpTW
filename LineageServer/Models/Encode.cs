using LineageServer.Interfaces;
using System;
using System.Text;

namespace LineageServer.Models
{
    public class Encode : IEncode
    {
        string IEncode.Encode(string oriString)
        {
            //byte[] buffer = Encoding.ASCII.GetBytes(oriString);
            //using (var sha = System.Security.Cryptography.SHA256.Create())
            //{
            //    buffer = sha.ComputeHash(buffer);
            //    sha.Dispose();
            //}
            //string s = Encoding.ASCII.GetString(buffer);
            return Convert.ToBase64String(GobalParameters.Encoding.GetBytes(oriString));
        }
    }
}
