using System.Security.Cryptography;

namespace SocialNetwork.Tests.Helpers
{
    public static class GuidConverter
    {
        public static Guid AsGuid(this string src)
        {
            var result = string.IsNullOrWhiteSpace(src)
                ? Guid.Empty
                : new Guid(MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(src)));
            return result;
        }
    }
}
