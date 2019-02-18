# BitBay.NET
.NET Standard 2.0 REST API wrapper for the BitBay cryptocurrency exchange

package - https://www.nuget.org/packages/BitBay.NET/

## Installation

Package is available in nuget repository

```code
Install-Package BitBay.NET
```

## Usage

```csharp

var client = new BitBayClient(new ClientConfiguration
{
    ApiKey = "apikey",
    ApiSecret = "apisecret"
});

var info = await client.GetInfoAsync();

...

```

## Release notes

### 1.0.4

- Removed unnecessary google.com call in GetTonce method
- Added optional parameters to all async and sync methods
