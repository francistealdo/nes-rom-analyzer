# NES ROM Analyzer

A .NET 10 application for inspecting Nintendo Entertainment System (NES) ROM files, reading cartridge metadata, and visualizing CHR graphics. The solution includes a reusable analysis library, a command-line interface, and a Windows desktop application built with WPF.

> **Project status:** Early development. iNES metadata reading and CHR ROM tile decoding are available; NES 2.0 support is currently limited.

## Features

- Open and inspect `.nes` ROM files.
- Validate the NES file signature and read the 16-byte ROM header.
- Detect iNES and NES 2.0 header formats (NES 2.0 detection only; extended fields are not yet fully decoded).
- Display PRG ROM and CHR ROM sizes, mapper number and known mapper names.
- Read battery-backed RAM, trainer, and nametable mirroring flags.
- Calculate full-file CRC32, MD5, and SHA-1 checksums.
- Decode CHR ROM into 8 × 8 pixel, 2-bit-per-pixel tiles.
- Preview tiles in the CLI and display a scrollable tile sheet in the WPF application.
- Render CHR graphics using a four-shade grayscale palette and nearest-neighbor scaling.

## Applications

### WPF desktop application

The Windows desktop application provides an **Open ROM** dialog, a panel with ROM metadata and checksums, and a scrollable CHR tile viewer. The current viewer uses a fixed tile size and grayscale palette.

### Command-line application

The CLI prompts for a ROM file path, prints header information and checksums, and shows sample CHR tiles using console characters.

## Solution structure

```text
nes-rom-analyzer/
├── NesRomAnalyzer.Core/      # ROM parsing, models, checksums, CHR decoding
│   ├── Enums/
│   ├── Models/
│   ├── Parsers/
│   └── Services/
├── NesRomAnalyzer.Cli/       # Console application
├── NesRomAnalyzer.Wpf/       # Windows desktop application
└── nes-rom-analyzer.slnx
```

The `Core` project contains the analysis logic and is referenced by both front ends. The WPF project handles the desktop user interface and bitmap rendering.

## Requirements

- Windows 10 or later for the WPF application.
- Visual Studio with the **.NET desktop development** workload.
- .NET 10 SDK for building the solution.
- Compatible .NET 10 runtime for running the applications.

## Getting started (Visual Studio)

1. Clone or download this repository.
2. Open `nes-rom-analyzer.slnx` in Visual Studio.
3. Restore NuGet packages if prompted.
4. Set `NesRomAnalyzer.Wpf` as the startup project.
5. Run the application, select **Open ROM...**, and choose a `.nes` file.

To use the console version, select `NesRomAnalyzer.Cli` as the startup project instead.

## CHR graphics

NES CHR ROM commonly stores 8 × 8 tiles in 16-byte, two-bitplane format. Each decoded pixel has a value from **0 to 3**. The WPF viewer maps these values to grayscale shades; they are **not necessarily the colors used by the game**, because the actual palette depends on how the game configures the NES PPU.

ROMs that use **CHR RAM** rather than stored CHR ROM may have no tiles available for static decoding. This does not mean the game has no graphics.

## Known limitations

- NES 2.0 extended sizes, mapper fields, and other metadata are not fully parsed yet.
- The parser recognizes the standard iNES signature, but ROM size and section validation need further hardening for malformed or truncated files.
- The WPF viewer currently uses a fixed grayscale palette, fixed zoom, and no individual tile selection or PNG export.
- No automated test project or CI workflow is currently included.
- This is a ROM inspection tool, **not an NES emulator**.

## Roadmap

- [ ] Improve header and file-boundary validation.
- [ ] Add automated tests for the header parser and CHR decoder.
- [ ] Complete NES 2.0 metadata parsing.
- [ ] Add configurable CHR zoom and palettes.
- [ ] Add tile selection, pixel inspection, and tile details.
- [ ] Export selected tiles and complete tile sheets as PNG.
- [ ] Add CHR bank navigation and richer ROM inspection features.
- [ ] Improve desktop UI separation as the application grows.

## ROM files

No commercial game ROMs are included in this repository. Use ROM files you are authorized to access or distribute, such as your own homebrew ROMs or appropriately licensed samples.

## Author

Developed by **Francis Tealdo Almeida Lima**.

GitHub: [@francistealdo](https://github.com/francistealdo)
