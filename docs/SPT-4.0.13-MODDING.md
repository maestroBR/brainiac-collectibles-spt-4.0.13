# Building mods for SPT 4.0.13

## Version-pinned references

This guide was checked against the official [4.0.13 server tag](https://github.com/SP-Tushonka/server-csharp/tree/4.0.13), commit `2891fd41fd07b6150a2192ac0d24adb93eb72862`, and [historical mod examples](https://github.com/SP-Tushonka/server-mod-examples/tree/7c78aa7e0a17). Current main branches can target newer, incompatible versions.

## Server project and lifecycle

Target `net9.0`. Reference `SPTarkov.Server.Core`, `SPTarkov.Common` and `SPTarkov.DI` at exact version `[4.0.13]`. Build a class library; do not ship the server runtime or copies of its core assemblies inside the mod.

Define one metadata record derived from `AbstractModMetadata`, overriding its required fields. Use a unique GUID, author, name, license, mod version and explicit SPT version range. The declared version must match the published Forge version. Set `IsBundleMod = true` for a bundle mod.

Register services through SPT dependency injection. The 4.0.13 load hook is `IOnLoad.OnLoad()` returning `Task`, rather than the newer cancellation-token hook. Code that reads or changes database items must run after `OnLoadOrder.PostDBModLoader`. Inject `DatabaseService` and retrieve templates/locations through its getters.

Primary references: [metadata](https://github.com/SP-Tushonka/server-csharp/blob/4.0.13/Libraries/SPTarkov.Server.Core/Models/Spt/Mod/AbstractModMetadata.cs), [load example](https://github.com/SP-Tushonka/server-mod-examples/blob/7c78aa7e0a17/8OnLoad/OnLoadExample.cs), [custom item example](https://github.com/SP-Tushonka/server-mod-examples/blob/7c78aa7e0a17/18CustomItemService/CustomItemServiceExample.cs).

## Custom items and translations

Prefer the native `CustomItemService.CreateItemFromClone` helper. Verify that the base template exists; preserve stable item IDs across updates; inspect the returned success/errors before using the result. Configure handbook placement, price, properties and localized names/descriptions explicitly. The 4.0.13 clone API does not expose every option from 4.1; compile against the pinned assemblies instead of copying newer examples blindly.

Locale dictionaries use the game's own language codes. Preserve formatting placeholders and test missing-language fallback. A server library compiling successfully does not verify translated text in the game UI.

## Bundles and client code

The native bundle loader reads `bundles.json` from the mod directory and resolves each key under `bundles/`. Preserve dependency keys, case and relative paths. Include every referenced bundle. Manifest parsing/file existence is a useful check, but only the target Unity client can establish whether models, shaders, materials and icons render correctly.

Reference: [4.0.13 BundleLoader](https://github.com/SP-Tushonka/server-csharp/blob/4.0.13/Libraries/SPTarkov.Server.Core/Loaders/BundleLoader.cs).

Client plugins belong to a separate BepInEx project built against the target game's Mono/Unity/BepInEx assemblies. The server's net9.0 target is not a client-plugin target. Harmony patches and obfuscated game members require target-client inspection. Fika requires separate host/client testing; server compilation alone establishes no multiplayer compatibility.

## Installation archive

For this release line, extract from the game root:

```text
SPT/user/mods/YourMod/YourMod.dll
SPT/user/mods/YourMod/bundles.json
SPT/user/mods/YourMod/bundles/...
BepInEx/plugins/YourClientPlugin/... (only if a client plugin exists)
```

The server scans each mod folder for DLLs at its top level. Avoid a versioned wrapper folder, nested DLL directories and the newer `SPT_Runtime/` root. Ship runtime content and licenses; exclude editor settings, tests, source build intermediates and private configuration from the install ZIP. Keep source available in the repository.

## Validation sequence

1. Restore/build exact dependencies and run isolated content/API checks.
2. Inspect ZIP paths, versions, asset references and checksums.
3. Start a real 4.0.13 test server with backed-up profiles and inspect registration errors.
4. In the matching client, inspect models/icons, all languages, item movement, loot, extraction and persistence after restart.
5. Test Fika host and client separately before declaring multiplayer support.

For this port, steps 1–2 have local evidence. Steps 3–5 remain pending. The installed SPT 4.1.6 game was not used as proof of 4.0.13 compatibility.
