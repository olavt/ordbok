using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ordbok.Models;
using Ordbok.Services;

namespace Ordbok.Pages;

public class IndexModel : PageModel
{
    private readonly OrdbokApiClient _apiClient;

    public static readonly Dictionary<string, string> DictionaryNames = new()
    {
        ["Bokmaalsordboka"] = "Bokmålsordboka",
        ["Nynorskordboka"] = "Nynorskordboka",
        ["NorskOrdbok"] = "Norsk Ordbok"
    };

    [BindProperty(SupportsGet = true)]
    public string? Word { get; set; }

    [BindProperty(SupportsGet = true)]
    public List<string> Dictionaries { get; set; } = ["Bokmaalsordboka", "Nynorskordboka", "NorskOrdbok"];

    public WordResult? Result { get; private set; }

    public bool Searched { get; private set; }

    public IndexModel(OrdbokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(Word) && Dictionaries.Count > 0)
        {
            Searched = true;
            Result = await _apiClient.GetWordAsync(Word.Trim(), Dictionaries);
        }
    }

    public static string FriendlyDictionaryName(string dict) =>
        DictionaryNames.TryGetValue(dict, out var name) ? name : dict;

    public static string FriendlyGender(string? gender) => gender switch
    {
        "Hankjoenn" => "masculine (m)",
        "Hokjoenn" => "feminine (f)",
        "Inkjekjoenn" => "neuter (n)",
        "HankjoennHokjoenn" => "masculine/feminine (m/f)",
        null => string.Empty,
        _ => gender
    };
}

