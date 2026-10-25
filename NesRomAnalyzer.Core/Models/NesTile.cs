namespace NesRomAnalyzer.Core.Models;

public sealed class NesTile
{
    public const int Width = 8;
    public const int Height = 8;
    public int Index { get; init; }
    public byte[,] Pixels { get; init; } = new byte[Height, Width];
}