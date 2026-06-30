# PMDG Livery Installer for Prepar3D

## Notice

For now this does requires one of the following:

1. Your user requires Modify permissions on the Prepar3D installation folder.
2. You are running this as an administrator.

This is because the tool needs to install liveries into the Prepar3D installation folder under:
- <Prepar3D installation folder>\SimObjects\Airplanes
- <Prepar3D installation folder>\PMDG\<Variant>\Aircraft

Writing to directories in `C:\Program Files` typically requires elevated or explicit permissions.

## Description

A tool for installing PMDG liveries for Lockheed Martin's Prepar3D v5.

Towards the end of May, PMDG disabled their web server for their OC2 platform. While it was clear that liveries would no longer be downloadable, it was less clear that users would be unable to install any liveries they backed up locally as decommissioning the web server rendered OC2 completely unusable.

This tool is a community contribution which allows you to install any liveries that you backed up for PMDG aircraft on Prepar3D.

If you are missing the liveries then you can still download these directly from PMDG without needing OC2.  
https://github.com/ColinM9991/Livery-Downloader

## Features

- Install liveries for PMDG aircraft
- Uninstall textures for PMDG aircraft
- Importing and nstalling 3rd party liveries

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

The logging and error handling isn't yet complete. My main focus has been to prove the concept and get a release out to support users.

I will implement Toast notifications and better logging/error handling in the future.

For now, please observe the log files in `%LOCALAPPDATA%\LiveryInstaller\current` and contact me with any issues so I can continue improving any unexpected scenarios.

If you are running the portable version you may get a crash when selecting your .ptp file. If this happens you may be missing [Microsoft Visual C++ 2010 SP1 Redistributable Package (x64)](https://www.microsoft.com/en-gb/download/details.aspx?id=26999).