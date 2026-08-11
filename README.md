# PMDG Livery Installer for Prepar3D

## Notice

Right now this only works on Windows 11. That's because the app uses a visual style (Fluent theme) that's built into Windows 11.

Support for Windows 10 is planned for a future update.

To use this tool, you'll need one of the following:

1. Your Windows user account has permission to make changes to your Prepar3D installation folder, or
2. You run the installer "as an administrator" (right-click the app and choose "Run as administrator")

This is because the tool needs to copy livery files into your Prepar3D installation folder, and Windows usually blocks programs from writing to that location unless you give it permission.

## Description

This tool installs PMDG liveries (aircraft paint schemes) for Lockheed Martin's Prepar3D v4, v5, and v6.

In late May, PMDG shut down the web server behind their OC2 platform which prevented anyone who had already backed up liveries locally to install them.

This tool lets you install those liveries again.

If you don't have any liveries backed up, you can still get them directly from PMDG without needing OC2: https://github.com/ColinM9991/Livery-Downloader

## Features

- Install liveries for PMDG aircraft
- Remove (uninstall) liveries for PMDG aircraft
- Import and install liveries made by third parties (not officially from PMDG)

Supported aircraft:
- 737
- 747
- 777

# Contributing

All developers are welcome to contribute to this project.

## Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Microsoft Visual C++ 2010 SP1 Redistributable Package (x64)](https://www.microsoft.com/en-gb/download/details.aspx?id=26999)
- [Visual Studio](https://visualstudio.microsoft.com/)
- [JetBrains Rider](https://www.jetbrains.com/rider/)

# Troubleshooting

Please check the log files in `%LOCALAPPDATA%\LiveryInstaller\current` and contact me if you hit any issues so I can keep improving how the tool handles them.

If you are running the portable version you may get a crash when selecting your .ptp file. If this happens you may be missing [Microsoft Visual C++ 2010 SP1 Redistributable Package (x64)](https://www.microsoft.com/en-gb/download/details.aspx?id=26999).
