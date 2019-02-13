# BitBay.NET
.NET Standard 2.0 API wrapper for the BitBay cryptocurrency exchange

package - https://www.nuget.org/packages/BitBay.NET/

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
