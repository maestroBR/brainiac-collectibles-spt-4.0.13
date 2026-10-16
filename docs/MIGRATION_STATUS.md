# SPT 4.0.13 migration status

## Pinned reference and license

- Gameplay/content reference: Brainiac Collectibles **1.4.0**, commit `6e98564` in [the stable repository](https://github.com/maestroBR/brainiac-collectibles).
- Code license: MIT, with original copyright retained.
- Asset license: Poly Haven CC0; full credits and source links are preserved in CREDITS.md/catalogue.json.
- Target API: [SPT 4.0.13 tag](https://github.com/SP-Tushonka/server-csharp/tree/4.0.13), commit `2891fd41fd07b6150a2192ac0d24adb93eb72862`; exact NuGet dependencies.
- Target core config: [4.0.13 core.json](https://github.com/SP-Tushonka/server-csharp/blob/4.0.13/Libraries/SPTarkov.Server.Assets/SPT_Data/configs/core.json), EFT `0.16.9.40087`.

Do not import changed prices, IDs, spawn weights or mechanics from another version. Treat the 1.4.0 catalogue and bundles as the fixed content reference.

## Adaptation matrix

| Stable 4.1.6 implementation | 4.0.13 adaptation |
| --- | --- |
| net10.0 and installed references | net9.0 with exact 4.0.13 packages |
| IModMetadata class | AbstractModMetadata record overrides |
| HasPrepatcher | IsBundleMod=true for the older bundle loader |
| IOnLoad.OnLoadAsync(CancellationToken) | IOnLoad.OnLoad() |
| OnLoadOrder.Preload + 5000 | OnLoadOrder.PostDBModLoader + 5000 |
| TemplateTable / LocationTable injection | DatabaseService.GetTemplates/GetLocations |
| Common.Models.Logging | Server.Core.Models.Utils logger |
| Services.Modding.Custom | Services.Mod |
| NewItemName clone option | Set registered template Name after clone creation |
| AddToWeaponShelf=false option | Not available in 4.0; retain the cloned base-item behavior |
| StaticLoot lazy transformer | Same native mechanism, same weights/map filters |

## Evidence and limits

- Release build passed without warnings.
- 1,060 checks passed against the pinned assemblies in .NET 9: all locales and required fields, short-name limits, missing/incomplete files, exact version range, metadata flag and load-hook shape.
- Source asset/catalogue parity is checked separately against the pinned reference. Existing bundles are not modified.
- No complete 4.0.13 installation was available for a server startup or client playthrough. No models, loot, extraction, inventory persistence or Fika behavior have been certified on the target game.

Next validation: start the target server with a backed-up test profile; verify item registration/bundle manifest; inspect each model and language; check safes on the configured maps, reload saved items and test Fika host/client separately. Do not promote this prerelease to stable on compile checks alone.
