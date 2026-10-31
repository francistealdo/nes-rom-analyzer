# NES ROM Analyzer

A .NET 10 tool for inspecting Nintendo Entertainment System (NES) ROM files, reading cartridge metadata, and decoding and exporting CHR graphics. The solution includes a reusable analysis library, a command-line interface, and a Windows desktop application built with WPF.

> **Project status:** Early development. Basic iNES inspection and CHR ROM tile decoding are implemented. NES 2.0 is detected, but its extended header fields are not fully supported yet.

## Features

- Open and inspect `.nes` ROM files.
- Validate the NES file signature and read the 16-byte ROM header.
- Identify iNES and NES 2.0 header formats (limited NES 2.0 support).
- Display PRG ROM and CHR ROM sizes, mapper number, and known mapper names.
- Read battery, trainer, and nametable mirroring flags.
- Calculate full-file CRC32, MD5, and SHA-1 checksums.
- Decode CHR ROM into 8 × 8 pixel tiles using the NES two-bitplane format.
- Preview sample tiles in the CLI and display a scrollable tile sheet in WPF.
- Render CHR tiles with a four-shade grayscale palette and nearest-neighbor scaling.
- **Export the entire rendered CHR tile sheet as a PNG image** from the WPF application.
- Use a custom application icon in the Windows desktop executable.

## Applications

### WPF desktop application

The WPF application provides:

- **Open ROM...** — select a `.nes` file.
- **ROM Information** — inspect format, mapper, PRG/CHR sizes, mirroring, battery/trainer flags, and checksums.
- **CHR Tile Viewer** — browse decoded tiles in a scrollable image, arranged in up to 16 tiles per row.
- **Export PNG...** — save the currently rendered tile sheet as a PNG image.

The current tile sheet is rendered at **3× scale** (24 × 24 displayed pixels per 8 × 8 tile) using four grayscale shades. The PNG export preserves this rendered layout and scale. Zoom and palette selection are not yet configurable.

### Command-line application

The CLI prompts for the path to a ROM file, prints cartridge information and checksums, and previews the first CHR tile and first non-empty tile using console characters.

## Solution structure

```text
nes-rom-analyzer/
├── NesRomAnalyzer.Core/
│   ├── Enums/                  # ROM format and mirroring modes
│   ├── Models/                 # ROM information, analysis, and tiles
│   ├── Parsers/                # iNES header parsing
│   └── Services/               # ROM reading, hashes, mapper names, CHR decoding
├── NesRomAnalyzer.Cli/         # Console application
├── NesRomAnalyzer.Wpf/         # Windows desktop application
│   ├── Resources/
│   │   └── nes_rom_analyzer.ico
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── README.md
└── nes-rom-analyzer.slnx
```

`NesRomAnalyzer.Core` contains reusable ROM-analysis logic shared by the CLI and WPF applications. The WPF project handles interface presentation, bitmap rendering, and PNG export.

## Requirements

- Windows and Visual Studio with the **.NET desktop development** workload to run the WPF application.
- **.NET 10 SDK** to build the solution.
- A compatible **.NET 10 runtime** to run the applications.

## Getting started (Visual Studio)

1. Clone or download the repository.
2. Open `nes-rom-analyzer.slnx` in Visual Studio.
3. Restore NuGet packages if prompted.
4. Set `NesRomAnalyzer.Wpf` as the startup project.
5. Run the application and select **Open ROM...**.
6. Choose a `.nes` file to inspect its header and, if available, its CHR graphics.
7. Select **Export PNG...** to save the tile sheet.

To run the console version instead, set `NesRomAnalyzer.Cli` as the startup project.

## How CHR decoding works

NES CHR ROM commonly stores graphics in **8 × 8 pixel tiles**, each occupying **16 bytes** in a two-bitplane format. For each row, one byte supplies the low bit of each pixel and a second byte supplies its high bit. Combining those bits produces a color index from **0 to 3**.

The WPF viewer maps these indices to four grayscale shades. **These are not necessarily the game's original colors**: actual colors depend on the palette configured by the game through the NES PPU.

The exported PNG is a **tile sheet**, not a reconstruction of in-game sprites or backgrounds. Sprite arrangements, tile positions, palette assignments, and game state are not reconstructed.

Some games use **CHR RAM** rather than storing CHR ROM in the file. Such ROMs may contain no statically decodable CHR tiles, even though graphics appear when the game is running.

## Known limitations

- NES 2.0 extended PRG/CHR sizes, mapper fields, and other header metadata are not fully decoded.
- Validation of truncated or malformed ROM sections needs improvement.
- The WPF viewer currently uses a fixed grayscale palette, fixed 3× scaling, and no individual tile selection.
- PNG export saves the entire *rendered* tile sheet; individual tile export and raw CHR export are not implemented.
- No automated test project or CI workflow is currently included.
- This is a **ROM inspection tool, not an NES emulator**.

## Roadmap

- [x] Read basic ROM headers and cartridge metadata.
- [x] Calculate CRC32, MD5, and SHA-1 checksums.
- [x] Decode CHR ROM tiles.
- [x] Build a WPF desktop viewer.
- [x] Export the rendered CHR tile sheet as PNG.
- [x] Add a custom desktop application icon.
- [ ] Improve header validation and truncated-file handling.
- [ ] Add automated tests for parsers and CHR decoding.
- [ ] Complete NES 2.0 metadata support.
- [ ] Add configurable CHR zoom and palettes.
- [ ] Add tile selection, enlarged preview, and pixel inspection.
- [ ] Export individual tiles and raw CHR data.
- [ ] Add CHR bank navigation and advanced ROM inspection.
- [ ] Refactor WPF state and commands into ViewModels as the UI grows.

## ROM files

No commercial game ROMs are included in this repository. Use ROM files you are authorized to access or distribute, including your own homebrew ROMs or appropriately licensed samples.

## Author

Developed by **Francis Tealdo**.

GitHub: [@francistealdo](https://github.com/francistealdo)
