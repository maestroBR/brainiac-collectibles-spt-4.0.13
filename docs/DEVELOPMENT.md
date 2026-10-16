# Building the experimental port

Use .NET SDK 9 or a newer SDK capable of targeting net9.0, Python 3 and network access to NuGet. Runtime checks should run with .NET 9 and ASP.NET Core 9 installed. Game binaries are not required for this build and are not distributed.

```sh
dotnet build src/Collectibles/Collectibles.csproj -c Release
dotnet run --project tests/Localization.csproj -c Release
python3 tools/validate.py
python3 tools/package.py
```

The package references are pinned to SPTarkov.Server.Core, Common and DI 4.0.13. Do not point this project at the 4.1.6 game libraries to make it compile. Build output is net9.0; the archive filename explicitly contains 4.0.13 and Experimental. Building never writes into an installed game.

The runtime bundles are unchanged from the pinned 1.4.0 reference. Rebuilding C# does not validate or rebuild Unity assets. Preserve CREDITS.md, MIT code licensing and CC0 asset provenance. See MIGRATION_STATUS.md for the adaptation matrix and pending in-game checks.
