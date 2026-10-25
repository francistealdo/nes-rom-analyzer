namespace NesRomAnalyzer.Core.Models;

public sealed class NesRomInfo
{
    public string FileName { get; init; } = string.Empty;
    public int PrgRomSizeKb { get; init; }
    public int ChrRomSizeKb { get; init; }
    public int Mapper { get; init; }
    public string MapperName { get; init; } = string.Empty;
    public bool HasBattery { get; init; }
    public bool HasTrainer { get; init; }
    public bool UsesVerticalMirroring { get; init; }
    public bool UsesFourScreenVram { get; init; }

    #region checksums

    public long FileSize { get; set; }
    public string Crc32 { get; set; } = string.Empty;
    public string Md5 { get; set; } = string.Empty;
    public string Sha1 { get; set; } = string.Empty;

    #endregion
}