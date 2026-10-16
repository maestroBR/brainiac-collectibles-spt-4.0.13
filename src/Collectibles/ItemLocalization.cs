using System.Text.Json;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace Maestro.BrainiacCollectibles;

public static class ItemLocalization
{
    private static readonly string[] Languages = ["en", "ge", "ru", "po", "ch", "fr", "es", "es-mx", "jp", "kr", "pl", "cz", "it", "hu", "ro", "sk", "tu"];

    public static Dictionary<string, Dictionary<string, LocaleDetails>> Load(
        string folder, IReadOnlyCollection<Collectible> entries)
    {
        var result = entries.ToDictionary(x => x.Slug, _ => new Dictionary<string, LocaleDetails>());
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        foreach (var language in Languages)
        {
            var file = System.IO.Path.Combine(folder, "locales", language + ".json");
            var translations = JsonSerializer.Deserialize<Dictionary<string, LocaleDetails>>(
                File.ReadAllText(file), options)
                ?? throw new InvalidDataException($"Empty localization file: {language}.");
            if (translations.Count != entries.Count || translations.Keys.Any(x => !result.ContainsKey(x)))
                throw new InvalidDataException($"Localization catalogue mismatch: {language}.");
            foreach (var entry in entries)
            {
                if (!translations.TryGetValue(entry.Slug, out var text)
                    || string.IsNullOrWhiteSpace(text.Name)
                    || string.IsNullOrWhiteSpace(text.ShortName)
                    || string.IsNullOrWhiteSpace(text.Description))
                    throw new InvalidDataException($"Missing item translation: {language}/{entry.Slug}.");
                result[entry.Slug][language] = text;
            }
        }
        return result;
    }
}
