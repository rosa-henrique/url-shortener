using Cassandra.Data.Linq;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Endpoints;

public static class UrlEndpoint
{
    public static WebApplication UseUrlEndpoint(this WebApplication app)
    {
        app.MapGet("{shortUrl}", async (string shortUrl, Table<Url> urlTable) =>
            {
                var url = await urlTable.FirstOrDefault(u => u.ShortUrl == shortUrl)
                                            .ExecuteAsync()
                                            .ConfigureAwait(false);
                
               return url is null ? Results.NotFound() : Results.Redirect(url.LongUrl);
            })
            .WithName("UrlShortener")
            .WithOpenApi();

        return app;
    }
}