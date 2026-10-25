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

    var rom = reader.Read(path);

    Console.WriteLine();
    Console.WriteLine($"File: {rom.FileName}");
    Console.WriteLine($"PRG ROM: {rom.PrgRomSizeKb} KB");
    Console.WriteLine($"CHR ROM: {rom.ChrRomSizeKb} KB");
    Console.WriteLine($"Mapper: {rom.Mapper} ({rom.MapperName})");
    Console.WriteLine($"Battery: {rom.HasBattery}");
    Console.WriteLine($"Trainer: {rom.HasTrainer}");
    Console.WriteLine($"Vertical Mirroring: {rom.UsesVerticalMirroring}");
    Console.WriteLine($"Four Screen VRAM: {rom.UsesFourScreenVram}");
    Console.WriteLine($"File Size: {rom.FileSize} bytes");
    Console.WriteLine($"CRC32: {rom.Crc32}");
    Console.WriteLine($"MD5: {rom.Md5}");
    Console.WriteLine($"SHA1: {rom.Sha1}");
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine($"Error: {ex.Message}");
}