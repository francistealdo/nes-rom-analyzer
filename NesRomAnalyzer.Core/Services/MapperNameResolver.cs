namespace NesRomAnalyzer.Core.Services;

public static class MapperNameResolver
{
    public static string Resolve(int mapper)
    {
        return mapper switch
        {
            0 => "NROM",
            1 => "MMC1",
            2 => "UxROM",
            3 => "CNROM",
            4 => "MMC3",
            5 => "MMC5",
            7 => "AxROM",
            9 => "MMC2",
            10 => "MMC4",
            11 => "Color Dreams",
            66 => "GxROM",
            _ => "Unknown"
        };
    }
}