using LineageServer.Interfaces;

namespace LineageServer.Models
{
    class Encode : IEncode
    {
        string IEncode.Encode(string oriString)
        {
            byte[] buffer = GobalParameters.Encoding.GetBytes(oriString);
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                buffer = sha.ComputeHash(buffer);
                sha.Dispose();
            }
            return GobalParameters.Encoding.GetString(buffer);
        }
    }
}
