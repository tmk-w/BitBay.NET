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

