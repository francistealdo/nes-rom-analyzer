using NesRomAnalyzer.Core.Services;

Console.Write("ROM path: ");

var path = Console.ReadLine();

if (string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("No ROM path provided.");
    return;
}

try
{
    var reader = new RomReader();

    var analysis = reader.Read(path);
    var rom = analysis.Info;

    Console.WriteLine();
    Console.WriteLine($"File: {rom.FileName}");
    Console.WriteLine($"Format: {rom.Format}");
    Console.WriteLine($"PRG ROM: {rom.PrgRomSizeKb} KB");
    Console.WriteLine($"CHR ROM: {rom.ChrRomSizeKb} KB");
    Console.WriteLine($"Mapper: {rom.Mapper} ({rom.MapperName})");
    Console.WriteLine($"Battery: {rom.HasBattery}");
    Console.WriteLine($"Trainer: {rom.HasTrainer}");
    Console.WriteLine($"Mirroring: {rom.Mirroring}");
    Console.WriteLine($"File Size: {rom.FileSize} bytes");
    Console.WriteLine($"CRC32: {rom.Crc32}");
    Console.WriteLine($"MD5: {rom.Md5}");
    Console.WriteLine($"SHA1: {rom.Sha1}");
    Console.WriteLine($"CHR Tiles: {analysis.Tiles.Count}");

    if (analysis.Tiles.Count > 0)
    {
        var tile = analysis.Tiles[0];

        Console.WriteLine();
        Console.WriteLine("First tile:");

        for (var row = 0; row < 8; row++)
        {
            for (var column = 0; column < 8; column++)
            {
                Console.Write(tile.Pixels[row, column]);
            }

            Console.WriteLine();
        }
    }

    var firstNonEmptyTile = analysis.Tiles.FirstOrDefault(tile =>
    {
        for (var row = 0; row < 8; row++)
        {
            for (var column = 0; column < 8; column++)
            {
                if (tile.Pixels[row, column] != 0)
                {
                    return true;
                }
            }
        }

        return false;
    });

    if (firstNonEmptyTile is not null)
    {
        Console.WriteLine();
        Console.WriteLine($"First non-empty tile: {firstNonEmptyTile.Index}");

        for (var row = 0; row < 8; row++)
        {
            for (var column = 0; column < 8; column++)
            {
                Console.Write(PixelToChar(firstNonEmptyTile.Pixels[row, column]));
            }

            Console.WriteLine();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine($"Error: {ex.Message}");
}

static char PixelToChar(byte value)
{
    return value switch
    {
        0 => ' ',
        1 => '░',
        2 => '▒',
        3 => '█',
        _ => '?'
    };
}