using SPTarkov.Server.Core.Models.Spt.Mod;
namespace Maestro.BrainiacCollectibles;
public sealed record Metadata:AbstractModMetadata {
 public override string ModGuid{get;init;}="project.brainiac.collectibles";
 public override string Name{get;init;}="Brainiac Collectibles — SPT 4.0.13 Experimental";
 public override string Author{get;init;}="Maestro";
 public override List<string>? Contributors{get;init;}=null;
 public override SemanticVersioning.Version Version{get;init;}=new("1.4.0");
 public override SemanticVersioning.Range SptVersion{get;init;}=new("=4.0.13");
 public override bool? IsBundleMod{get;init;}=true;
 public override List<string>? Incompatibilities{get;init;}=null;
 public override Dictionary<string,SemanticVersioning.Range>? ModDependencies{get;init;}=null;
 public override string? Url{get;init;}="https://github.com/maestroBR/brainiac-collectibles-spt-4.0.13";
 public override string License{get;init;}="MIT";
}
