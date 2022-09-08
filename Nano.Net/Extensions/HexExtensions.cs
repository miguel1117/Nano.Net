using System;
using System.Linq;
using System.Text;

namespace Nano.Net.Extensions;

public static class HexExtensions
{
    public static byte[] HexToBytes(this string hex)
    {
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Hex string length isn't valid.");

        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    public static string BytesToHex(this byte[] bytes)
    {
        var hex = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
            hex.AppendFormat("{0:x2}", b);

        return hex.ToString().ToUpper();
    }
}
