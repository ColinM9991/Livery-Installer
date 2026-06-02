# PMDG Livery Installer for Prepar3D

## Description

A tool for installing PMDG liveries for Lockheed Martin's Prepar3D v5.

Towards the end of May, PMDG disabled their web server for their OC2 platform. While it was clear that liveries would no longer be downloadable, it was less clear that users would be unable to install any liveries they backed up locally as decommissioning the web server rendered OC2 completely unusable.

This tool is a community contribution which allows you to install any liveries that you backed up for PMDG aircraft on Prepar3D.

## Disclaimer (READ ME)

For PMDG: I have ensured that this tool does not violate or infringe upon your rights. I am not redistributing PMDG's liveries nor am I disclosing the passphrase used to decrypt the PTP files.

While this application does utilize CabLib, I have researched online to discover that this is not PMDG's intellectual property.

This tool does not redistribute PMDGs liveries (since they are copyrighted), nor does it come supplied with the passphrase required to decrypt PMDG's PTP files.

If you did not back up the liveries then this tool will not work. The liveries need to be structured in a specific way as outlined in [liveryConfiguration.json](src/LiveryInstaller.UI/liveryConfiguration.json)

## Features

- Install liveries for PMDG aircraft
- Uninstall textures for PMDG aircraft

Not yet supported:
- Installing 3rd party liveries

Supported aircraft:
- DC-6
- 737
- 747
- 777

# Contributing

All developers are welcome to contribute to this project.

## Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Visual Studio](https://visualstudio.microsoft.com/)
- [JetBrains Rider](https://www.jetbrains.com/rider/)