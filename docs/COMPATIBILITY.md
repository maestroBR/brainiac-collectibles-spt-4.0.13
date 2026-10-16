# Experimental compatibility

| Area | Status |
| --- | --- |
| Target SPT version | Exactly 4.0.13 |
| Target server framework | .NET 9 |
| Target API references | Exact official 4.0.13 NuGet packages |
| Release build | Passed |
| Isolated localization and metadata/API checks | 1,060 passed under .NET 9 |
| Catalogue, bundles and locales | 30 items/bundles; 17 locales validated |
| Full target server startup | Not tested |
| Item prefab rendering in the 4.0.13 client | Not tested |
| Actual loot discovery and reward transfers | Not tested |
| Fika host/client and reconnects | Not tested |

The official 4.0.13 server config identifies EFT 0.16.9.40087. The existing asset bundles have not been re-exported specifically for that client. Never infer client compatibility from a successful server build.

Do not load the 4.0.13 and 4.1.6 variants together: they intentionally retain the same GUID and item IDs. No game assemblies, target server data, runtime libraries or player profiles are distributed.
