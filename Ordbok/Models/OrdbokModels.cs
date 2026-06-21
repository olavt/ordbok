namespace Ordbok.Models;

// GraphQL response envelope
public record GraphQlResponse<T>(T? Data);

// Word lookup
public record WordData(WordResult? Word);

public record WordResult(string Word, List<string> Dictionaries, List<Article> Articles);

public record Article(
    int Id,
    string Dictionary,
    string? WordClass,
    string? Gender,
    List<LemmaEntry> Lemmas,
    List<FlatDefinition> FlatDefinitions,
    List<DefinitionContent> Etymology);

public record LemmaEntry(string Lemma, List<Paradigm> Paradigms);

public record Paradigm(List<string> Tags, List<Inflection> Inflections);

public record Inflection(string WordForm, List<string> Tags);

public record FlatDefinition(
    int? ParentIndex,
    List<DefinitionContent> Content,
    List<DefinitionContent> Examples);

public record DefinitionContent(string TextContent);

// Suggestions / autocomplete
public record SuggestionsData(SuggestionsResult Suggestions);

public record SuggestionsResult(
    List<SuggestionWord> Exact,
    List<SuggestionWord> Inflections,
    List<SuggestionWord> Freetext,
    List<SuggestionWord> Similar);

public record SuggestionWord(string Word);
