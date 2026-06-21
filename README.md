# Ordbok

A Norwegian dictionary lookup web application built with **ASP.NET Core 10 Razor Pages**, powered by the free [Ordbok API](https://ordbokapi.org/) GraphQL endpoint.

## Features

- **Word lookup** across Bokmålsordboka, Nynorskordboka, and Norsk Ordbok simultaneously.
- **Definitions** with usage examples shown in italics.
- **Inflection tables** for each word form and paradigm.
- **Etymology** when available.
- **Autocomplete** suggestions as you type (powered by the Ordbok suggestions API).
- No authentication or API key required — the Ordbok API is publicly available.

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core 10, Razor Pages |
| Language | C# 13, nullable reference types |
| HTTP / GraphQL | `IHttpClientFactory` with a typed `OrdbokApiClient` using `System.Net.Http.Json` |
| Serialization | `System.Text.Json` with `PropertyNameCaseInsensitive = true` |
| UI | Bootstrap 5 (bundled by template) |
| Data source | [Ordbok API](https://api.ordbokapi.org/graphql) — GraphQL, no authentication |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Running Locally

```bash
cd Ordbok
dotnet run
```

Then open `https://localhost:5001` (or the URL printed in the terminal).

## Project Structure

```
Ordbok/
├── Models/
│   └── OrdbokModels.cs        # C# record types for GraphQL responses
├── Services/
│   └── OrdbokApiClient.cs     # Typed HTTP client — word lookup & suggestions
├── Pages/
│   ├── Index.cshtml            # Search form and results page
│   └── Index.cshtml.cs         # Page model (calls OrdbokApiClient)
└── Program.cs                  # Service registration, minimal-API suggest endpoint
```

## Dictionaries

| GraphQL enum value | Display name |
|---|---|
| `Bokmaalsordboka` | Bokmålsordboka |
| `Nynorskordboka` | Nynorskordboka |
| `NorskOrdbok` | Norsk Ordbok |

## API Endpoints

- `GET /` — Search page (accepts `?word=...&dictionaries=...` query parameters)
- `GET /api/suggest?word=...&dictionaries=...` — Returns a JSON array of autocomplete suggestions

## License

This project is open source. The dictionary data is provided by [Ordbok API](https://ordbokapi.org/) under their respective terms.

