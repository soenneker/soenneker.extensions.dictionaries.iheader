[![](https://img.shields.io/nuget/v/soenneker.extensions.dictionaries.iheader.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.dictionaries.iheader/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.dictionaries.iheader/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.dictionaries.iheader/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.dictionaries.iheader.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.dictionaries.iheader/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.dictionaries.iheader/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.dictionaries.iheader/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.Dictionaries.IHeader
Serializes ASP.NET Core request or response headers to compact JSON without flattening multi-valued headers.

## Installation

```bash
dotnet add package Soenneker.Extensions.Dictionaries.IHeader
```

## Usage

```csharp
using Soenneker.Extensions.Dictionaries.IHeader;

IHeaderDictionary headers = httpContext.Request.Headers;
string json = headers.ToJsonString();
```

Given one `Accept` value and two `X-Tag` values, the shape is:

```json
{"Accept":"application/json","X-Tag":["blue","green"]}
```

Headers with zero or one value are written as JSON strings; headers with multiple values are written as arrays. The method preserves the collection's enumeration order, emits no indentation, and does not mutate the headers. The source must be non-null.
