namespace NesRomAnalyzer.Core.Models;

public sealed class NesRomInfo
{
    public string FileName { get; init; } = string.Empty;
    public int PrgRomSizeKb { get; init; }
    public int ChrRomSizeKb { get; init; }
    public int Mapper { get; init; }
    public bool HasBattery { get; init; }
    public bool HasTrainer { get; init; }
    public bool UsesVerticalMirroring { get; init; }
    public bool UsesFourScreenVram { get; init; }
}