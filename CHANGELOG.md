# Changelog

## 1.4.0 — Experimental — 2026-10-16

Experimental SPT 4.0.13 backport, based on Brainiac Collectibles 1.4.0.

- Correct the installation root to `SPT/user/mods` for SPT 4.0.x.
- Match the Forge published version exactly: DLL metadata now declares `1.4.0`. Experimental status is retained in the release label instead of the version suffix.

- Target .NET 9 and pin the SPT Core/Common/DI packages to exactly 4.0.13.
- Use the 4.0 metadata record, bundle-mod flag and exact SPT version requirement.
- Adapt the load hook to IOnLoad.OnLoad and the 4.0 post-database load order.
- Access templates and locations through DatabaseService and the 4.0 CustomItemService namespace.
- Adapt clone registration to the 4.0 API and preserve the internal template names after registration.
- Preserve all 30 IDs, bundles, prices, loot weights and 17 locales from the reference version.
- Add target metadata/API checks to the localization regression runner.

Build and isolated tests passed. Full target-server startup, bundle rendering, loot and Fika gameplay remain unverified. This is an experimental prerelease, not a stable release.
