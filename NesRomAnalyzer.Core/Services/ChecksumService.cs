using System.Security.Cryptography;
using System.IO.Hashing;

namespace NesRomAnalyzer.Core.Services;

public static class ChecksumService
{
    public static string CalculateMd5(byte[] data)
    {
        var hash = MD5.HashData(data);
        return Convert.ToHexString(hash);
    }

    public static string CalculateSha1(byte[] data)
    {
        var hash = SHA1.HashData(data);
        return Convert.ToHexString(hash);
    }

    public static string CalculateCrc32(byte[] data)
    {
        var hash = Crc32.Hash(data);
        return Convert.ToHexString(hash);
    }
}