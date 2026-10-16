# Brainiac Collectibles — SPT 4.0.13 Experimental

**Experimental backport for SPT 4.0.13 only.** This is a separate test build, not the stable SPT 4.1.6 release.

[Experimental downloads](https://github.com/maestroBR/brainiac-collectibles-spt-4.0.13/releases) · [Stable project](https://github.com/maestroBR/brainiac-collectibles)

Includes the same **30 collectibles**, models, item IDs and **17 languages** as Brainiac Collectibles 1.4.0. Names, prices and loot weights have not been rebalanced.

## Status

Version: **1.4.0-alpha.1**, built for **SPT 4.0.13 / .NET 9** using exact 4.0.13 NuGet dependencies. The port adapts server metadata, load order and item-registration APIs; it is more than a version-number change.

Release build and 1,060 isolated checks passed against the target libraries under .NET 9. A complete SPT 4.0.13 server startup, in-game model rendering, loot discovery and Fika gameplay have **not** been verified. The bundles are reused unchanged and still need testing with the 4.0.13 client.

## Testing the build

1. Back up your SPT 4.0.13 profiles and existing mod folder. Prefer a separate test profile.
2. Close the game and server.
3. Extract `Brainiac-Collectibles-1.4.0-alpha.1-SPT-4.0.13-Experimental.zip` into your **SPT 4.0.13** game root.
4. The mod belongs in `SPT_Runtime/user/mods/Maestro-BrainiacCollectibles/`.
5. Start your server and check its logs before launching the game.

Do not install this over the stable 4.1.6 build or load both variants together. The mod GUID and item IDs are deliberately unchanged. This does not make profiles interchangeable between SPT versions. Close both processes and restore your backup if testing fails.

The mod is standalone. Weekend collection addons built for SPT 4.1.6 are **not** included or certified for 4.0.13. With Fika, each player's bundles and mod version must match; multiplayer is unverified.

## Reporting issues

Include the exact mod/SPT version, reproduction steps and relevant server/client logs. Please report missing models, missing translations, startup errors and loot problems separately. Avoid posting complete profiles or account/session tokens.

## Credits

Project Brainiac. Original item models and textures: **Poly Haven, CC0**, with source/author credits in [CREDITS.md](CREDITS.md). Code: [MIT](LICENSE).

[Port status and adaptation matrix](docs/MIGRATION_STATUS.md) · [Build instructions](docs/DEVELOPMENT.md)
