using NesRomAnalyzer.Core.Models;
using NesRomAnalyzer.Core.Parsers;

namespace NesRomAnalyzer.Core.Services;

public sealed class RomReader
{
    private const int InesHeaderSize = 16;

    private readonly InesHeaderParser _headerParser = new();

    public NesRomAnalysis Read(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("A ROM file path is required.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("ROM file was not found.", filePath);
        }

        var data = File.ReadAllBytes(filePath);

        if (data.Length < InesHeaderSize)
        {
            throw new InvalidDataException("The file is too small to contain a valid iNES header.");
        }

        var romInfo = _headerParser.Parse(Path.GetFileName(filePath), data.AsSpan(0, InesHeaderSize));

        var offset = InesHeaderSize;

        if (romInfo.HasTrainer)
        {
            offset += 512;
        }

        offset += romInfo.PrgRomSizeKb * 1024;

        romInfo.FileSize = data.Length;
        romInfo.Crc32 = ChecksumService.CalculateCrc32(data);
        romInfo.Md5 = ChecksumService.CalculateMd5(data);
        romInfo.Sha1 = ChecksumService.CalculateSha1(data);

        var tiles = ReadTiles(data, romInfo);

        return new NesRomAnalysis
        {
            Info = romInfo,
            Tiles = tiles
        };
    }

    private IReadOnlyList<NesTile> ReadTiles(byte[] data, NesRomInfo romInfo)
    {
        if (romInfo.ChrRomSizeKb == 0)
        {
            return Array.Empty<NesTile>();
        }

        var offset = InesHeaderSize;

        if (romInfo.HasTrainer)
        {
            offset += 512;
        }

        offset += romInfo.PrgRomSizeKb * 1024;

        var chrSize = romInfo.ChrRomSizeKb * 1024;
        var chrData = data.AsSpan(offset, chrSize);
        var decoder = new ChrTileDecoder();

        return decoder.Decode(chrData);
    }
}