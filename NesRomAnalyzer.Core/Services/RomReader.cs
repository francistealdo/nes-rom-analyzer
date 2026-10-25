using NesRomAnalyzer.Core.Models;
using NesRomAnalyzer.Core.Parsers;

namespace NesRomAnalyzer.Core.Services;

public sealed class RomReader
{
    private const int InesHeaderSize = 16;

    private readonly InesHeaderParser _headerParser = new();

    public NesRomInfo Read(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("A ROM file path is required.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("ROM file was not found.", filePath);
        }

        using var stream = File.OpenRead(filePath);

        if (stream.Length < InesHeaderSize)
        {
            throw new InvalidDataException("The file is too small to contain a valid iNES header.");
        }

        Span<byte> header = stackalloc byte[InesHeaderSize];

        var bytesRead = stream.Read(header);

        if (bytesRead != InesHeaderSize)
        {
            throw new InvalidDataException("Unable to read the complete iNES header.");
        }

        return _headerParser.Parse(Path.GetFileName(filePath), header);
    }
}