namespace NesRomAnalyzer.Core.Models;

public sealed class NesRomAnalysis
{
    public required NesRomInfo Info { get; init; }
    public IReadOnlyList<NesTile> Tiles { get; init; } = [];
}