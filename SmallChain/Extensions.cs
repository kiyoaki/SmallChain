using System;
using System.Collections.Generic;
using System.Text;

namespace SmallChain
{
    public static class Extensions
    {
        private static readonly DateTime UnixEpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal static long ToUnixTime(this DateTime datetime)
        {
            return (long)datetime.ToUniversalTime().Subtract(UnixEpochDateTime).TotalSeconds;
        }

        internal static string ToHexString(this IReadOnlyCollection<byte> bytes)
        {
            var sb = new StringBuilder(bytes.Count * 2);
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
