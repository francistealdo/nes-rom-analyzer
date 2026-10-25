using NesRomAnalyzer.Core.Models;

namespace NesRomAnalyzer.Core.Parsers;

public sealed class InesHeaderParser
{
    private static readonly byte[] NesSignature =
    [
        0x4E, // N
        0x45, // E
        0x53, // S
        0x1A
    ];

    public NesRomInfo Parse(string fileName, ReadOnlySpan<byte> header)
    {
        if (header.Length < 16)
        {
            throw new InvalidDataException("Invalid iNES header.");
        }

        if (!HasValidSignature(header))
        {
            throw new InvalidDataException("The file is not a valid NES ROM.");
        }

        var prgRomSizeKb = header[4] * 16;
        var chrRomSizeKb = header[5] * 8;

        var flags6 = header[6];
        var flags7 = header[7];

        // The mapper number is stored in the upper 4 bits of flags 6 and flags 7
        var mapper = (flags6 >> 4) | (flags7 & 0xF0);

        var hasBattery = (flags6 & 0b0000_0010) != 0;
        var hasTrainer = (flags6 & 0b0000_0100) != 0;
        var usesVerticalMirroring = (flags6 & 0b0000_0001) != 0;
        var usesFourScreenVram = (flags6 & 0b0000_1000) != 0;

        return new NesRomInfo
        {
            FileName = fileName,
            PrgRomSizeKb = prgRomSizeKb,
            ChrRomSizeKb = chrRomSizeKb,
            Mapper = mapper,
            HasBattery = hasBattery,
            HasTrainer = hasTrainer,
            UsesVerticalMirroring = usesVerticalMirroring,
            UsesFourScreenVram = usesFourScreenVram
        };
    }

    private static bool HasValidSignature(ReadOnlySpan<byte> header)
    {
        return header[0] == NesSignature[0] &&
               header[1] == NesSignature[1] &&
               header[2] == NesSignature[2] &&
               header[3] == NesSignature[3];
    }
}