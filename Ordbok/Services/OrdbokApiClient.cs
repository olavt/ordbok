using System.Net.Http.Json;
using System.Text.Json;
using Ordbok.Models;

namespace Ordbok.Services;

public class OrdbokApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private const string WordLookupQuery = """
        query Lookup($word: String!, $dictionaries: [Dictionary!]!) {
          word(word: $word, dictionaries: $dictionaries) {
            word
            dictionaries
            articles {
              id
              dictionary
              wordClass
              gender
              lemmas {
                lemma
                paradigms {
                  tags
                  inflections { wordForm tags }
                }
              }
              flatDefinitions {
                parentIndex
                content { textContent }
                examples { textContent }
              }
              etymology { textContent }
            }
          }
        }
        """;

    private const string SuggestionsQuery = """
        query Suggest($word: String!, $dictionaries: [Dictionary!]!, $maxCount: Int) {
          suggestions(word: $word, dictionaries: $dictionaries, maxCount: $maxCount) {
            exact { word }
            inflections { word }
            freetext { word }
            similar { word }
          }
        }
        """;

    public OrdbokApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WordResult?> GetWordAsync(string word, IEnumerable<string> dictionaries)
    {
        var request = new
        {
            query = WordLookupQuery,
            variables = new { word, dictionaries }
        };

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress, request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GraphQlResponse<WordData>>(JsonOptions);
        return result?.Data?.Word;
    }

    public async Task<List<string>> GetSuggestionsAsync(
        string word, IEnumerable<string> dictionaries, int maxCount = 10)
    {
        var request = new
        {
            query = SuggestionsQuery,
            variables = new { word, dictionaries, maxCount }
        };

        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress, request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GraphQlResponse<SuggestionsData>>(JsonOptions);
        var suggestions = result?.Data?.Suggestions;
        if (suggestions is null)
            return [];

        return suggestions.Exact
            .Concat(suggestions.Inflections)
            .Concat(suggestions.Freetext)
            .Concat(suggestions.Similar)
            .Select(s => s.Word)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(maxCount)
            .ToList();
    }
}
