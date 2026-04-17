# valheim-hex-libs

A .NET Framework 4.8 library providing configuration and utility features for Valheim BepinEx mods.

## Overview

This library provides a foundational framework for building BepinEx plugins for Valheim with standardized configuration management and utilities.

## Requirements

- .NET Framework 4.8
- BepinEx (for Valheim modding)

## Project Structure

```
HexLib.BepinEx.Configuration/
├── Configuration.cs
├── Properties/
│   └── AssemblyInfo.cs
└── HexLib.BepinEx.Configuration.csproj
```

## Getting Started

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/guillenjgg/valheim-hex-libs.git
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. The compiled DLL will be available in the `bin` directory.

## Features

- Configuration management for BepinEx mods
- Extensible architecture for additional utilities

## Usage

Reference this library in your BepinEx plugin project and use the configuration classes.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to contribute to this project.

## License

Check the repository for license information.

## Support

For issues and feature requests, please open an issue on the [GitHub repository](https://github.com/guillenjgg/valheim-hex-libs).
