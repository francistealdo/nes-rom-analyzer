
using Microsoft.Win32;
using NesRomAnalyzer.Core.Models;
using NesRomAnalyzer.Core.Services;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NesRomAnalyzer.Wpf;

public partial class MainWindow : Window
{
    private readonly RomReader _romReader = new();

    private const int TilesPerRow = 16;
    private const int PixelScale = 3;

    private static readonly byte[][] Palette =
    [
        [32, 32, 32, 255],
        [100, 100, 100, 255],
        [180, 180, 180, 255],
        [255, 255, 255, 255]
    ];

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenRom_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open NES ROM",
            Filter = "NES ROM files (*.nes)|*.nes|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            var analysis = _romReader.Read(dialog.FileName);

            DisplayRomInfo(analysis.Info);
            DisplayChrTiles(analysis.Tiles);

            FileNameText.Text = analysis.Info.FileName;
            StatusText.Text = "ROM loaded successfully.";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Failed to load ROM.";

            MessageBox.Show(ex.Message, "ROM Analysis Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DisplayRomInfo(NesRomInfo rom)
    {
        FormatText.Text = rom.Format.ToString();
        MapperText.Text = $"{rom.Mapper} - {rom.MapperName}";
        PrgText.Text = $"{rom.PrgRomSizeKb} KB";
        ChrText.Text = $"{rom.ChrRomSizeKb} KB";
        MirroringText.Text = rom.Mirroring.ToString();

        FlagsText.Text = $"Battery: {rom.HasBattery} / Trainer: {rom.HasTrainer}";

        CrcText.Text = rom.Crc32;
        Md5Text.Text = rom.Md5;
        ShaText.Text = rom.Sha1;
    }

    private void DisplayChrTiles(IReadOnlyList<NesTile> tiles)
    {
        TileCountText.Text = $"{tiles.Count} tiles";

        ChrImage.Source = tiles.Count == 0 ? null : CreateChrBitmap(tiles);
    }

    private static WriteableBitmap CreateChrBitmap(
        IReadOnlyList<NesTile> tiles)
    {
        var columns = Math.Min(TilesPerRow, tiles.Count);
        var rows = (tiles.Count + columns - 1) / columns;

        var tileWidth = NesTile.Width * PixelScale;
        var tileHeight = NesTile.Height * PixelScale;

        var width = columns * tileWidth;
        var height = rows * tileHeight;

        const int bytesPerPixel = 4;
        var stride = checked(width * bytesPerPixel);
        var pixels = new byte[checked(stride * height)];

        foreach (var tile in tiles)
        {
            var tileX = (tile.Index % columns) * tileWidth;
            var tileY = (tile.Index / columns) * tileHeight;

            for (var y = 0; y < NesTile.Height; y++)
            {
                for (var x = 0; x < NesTile.Width; x++)
                {
                    var color = Palette[tile.Pixels[y, x]];

                    for (var sy = 0; sy < PixelScale; sy++)
                    {
                        for (var sx = 0; sx < PixelScale; sx++)
                        {
                            var px = tileX + x * PixelScale + sx;
                            var py = tileY + y * PixelScale + sy;

                            var index = py * stride + px * bytesPerPixel;

                            // BGRA32
                            pixels[index] = color[0];
                            pixels[index + 1] = color[1];
                            pixels[index + 2] = color[2];
                            pixels[index + 3] = color[3];
                        }
                    }
                }
            }
        }

        var bitmap = new WriteableBitmap(
            width,
            height,
            96,
            96,
            PixelFormats.Bgra32,
            null);

        bitmap.WritePixels(
            new Int32Rect(0, 0, width, height),
            pixels,
            stride,
            0);

        bitmap.Freeze();

        return bitmap;
    }
}
