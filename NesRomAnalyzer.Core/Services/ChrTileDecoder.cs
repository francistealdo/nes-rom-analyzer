using NesRomAnalyzer.Core.Models;

namespace NesRomAnalyzer.Core.Services;

public sealed class ChrTileDecoder
{
    private const int BytesPerTile = 16;

    public IReadOnlyList<NesTile> Decode(ReadOnlySpan<byte> chrData)
    {
        if (chrData.Length % BytesPerTile != 0)
        {
            throw new InvalidDataException("CHR data size must be a multiple of 16 bytes.");
        }

        var tileCount = chrData.Length / BytesPerTile;
        var tiles = new List<NesTile>(tileCount);

        for (var tileIndex = 0; tileIndex < tileCount; tileIndex++)
        {
            var tileData = chrData.Slice(tileIndex * BytesPerTile, BytesPerTile);
            tiles.Add(DecodeTile(tileIndex, tileData));
        }

        return tiles;
    }

    private static NesTile DecodeTile(int index, ReadOnlySpan<byte> data)
    {
        var pixels = new byte[8, 8];

        for (var row = 0; row < 8; row++)
        {
            var lowPlane = data[row];
            var highPlane = data[row + 8];

            for (var column = 0; column < 8; column++)
            {
                var bit = 7 - column;

                var lowBit = (lowPlane >> bit) & 1;
                var highBit = (highPlane >> bit) & 1;
                var colorIndex = (highBit << 1) | lowBit;

                pixels[row, column] = (byte)colorIndex;
            }
        }

        return new NesTile
        {
            Index = index,
            Pixels = pixels
        };
    }
}