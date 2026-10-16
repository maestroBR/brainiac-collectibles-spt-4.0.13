using Path = System.IO.Path;
using System.Text.Json;
using System.Reflection;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Mod;
namespace Maestro.BrainiacCollectibles;
public sealed record Collectible(string Slug,string Id,string Category,string Name,string ShortName,string Description,string Tier,int Price,double Weight,int Width,int Height,string Bundle);
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 5000)]
public sealed class Mod(CustomItemService itemService, DatabaseService database, ISptLogger<Mod> logger) : IOnLoad
{
    public Task OnLoad()
    {
        var templates = database.GetTemplates();
        var locations = database.GetLocations();
        var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var entries = JsonSerializer.Deserialize<List<Collectible>>(File.ReadAllText(Path.Combine(folder,"catalogue.json")),new JsonSerializerOptions { PropertyNameCaseInsensitive=true })!;
        if(entries.Count!=30 || entries.Select(x=>x.Id).Distinct().Count()!=30) throw new InvalidDataException("Catálogo Brainiac inválido.");
        var localizedItems = ItemLocalization.Load(folder, entries);
        // Do not create unusable items when assets have not been built and distributed.
        if(entries.Any(x=>!File.Exists(Path.Combine(folder,"bundles",x.Bundle))))
        { logger.Warning("[Brainiac Collectibles] Assets incompletos; nenhum item foi cadastrado. Concluir os bundles antes da ativação."); return Task.CompletedTask; }
        if(entries.Any(x=>templates.Items.ContainsKey(new MongoId(x.Id)))) throw new InvalidDataException("Colisão de IDs de colecionáveis Brainiac; revisar instalação.");
        var baseItemId = new MongoId("59faf7ca86f7740dbe19f6c2");
        if (!templates.Items.ContainsKey(baseItemId))
            throw new InvalidDataException("SPT 4.0.13 base item template is missing; item registration aborted.");
        foreach(var entry in entries)
        {
            var created = itemService.CreateItemFromClone(new NewItemFromCloneDetails {
                NewId=entry.Id,ItemTplToClone=new MongoId("59faf7ca86f7740dbe19f6c2"),
                ParentId="57864a3d24597754843f8721",
                HandbookParentId="5b47574386f77428ca22b2f1",HandbookPriceRoubles=entry.Price,FleaPriceRoubles=entry.Price,
                Locales=localizedItems[entry.Slug],
                OverrideProperties=new TemplateItemProperties { Prefab=new(){Path=entry.Bundle,Rcid=""},Weight=entry.Weight,Width=entry.Width,Height=entry.Height,CanSellOnRagfair=false,ExaminedByDefault=false,BackgroundColor=entry.Category=="exotic"?"violet":entry.Category=="military"?"yellow":"red",QuestItem=false,StackMaxSize=1 }
            });
            if (created.Success != true)
                throw new InvalidDataException("Collectible registration failed: " + entry.Slug);
            // The 4.0 API has no NewItemName option; set the registered template's internal name.
            templates.Items[new MongoId(entry.Id)].Name = "brainiac_collectible_" + entry.Slug;
        }
        foreach(var (locationKey, location) in locations.GetDictionary())
        {
            var mapName=locationKey.ToLowerInvariant();
            location.StaticLoot?.AddTransformer(loot=> {
                if(loot is null) return loot;
                foreach(var (containerId, distribution) in loot)
                {
                    if(!templates.Items.TryGetValue(containerId,out var container) || !(container.Name?.StartsWith("Safe",StringComparison.OrdinalIgnoreCase)??false)) continue;
                    var existing=distribution.ItemDistribution.ToList();
                    // Bounded weights relative to each container pool, not independent spawn percentages.
                    float total=existing.Sum(x=>Math.Max(0,x.RelativeProbability??0));
                    if(total<=0) continue;
                    foreach(var entry in entries)
                    {
                        if(entry.Category=="exotic" && mapName is not ("laboratory" or "labyrinth" or "rezervbase"))continue;
                        if(entry.Category=="military" && mapName is not ("rezervbase" or "woods" or "lighthouse" or "bigmap"))continue;
                        float weight=entry.Tier switch {"rare"=>.0009f,"legendary"=>.00045f,"exotic"=>.0002f,_=>.00008f};
                        if(!existing.Any(x=>x.Tpl==new MongoId(entry.Id)))existing.Add(new(){Tpl=new MongoId(entry.Id),RelativeProbability=total*weight});
                    }
                    distribution.ItemDistribution=existing;
                }
                return loot;
            });
        }
        logger.Info("[Brainiac Collectibles] 30 itens cadastrados; loot raro em cofres por região. Perfis não modificados.");
        return Task.CompletedTask;
    }
}
