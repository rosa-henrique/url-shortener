using System.Security.Cryptography;
using System.Text;
using Cassandra.Data.Linq;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Dto;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Endpoints;

public static class AdminUrlEndpoint
{
    private static readonly char[] Characters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    public static WebApplication UseAdminUrlEndpoint(this WebApplication app)
    {
        var mapGroup = app.MapGroup("admin");

        mapGroup.MapPost("url",
                async ([FromBody] CreateShortUrlRequest request, Table<Url> urls, IConfiguration configuration) =>
                {
                    if (string.IsNullOrEmpty(request?.LongUrl))
                        return Results.Problem(detail: "Long url is required", statusCode: 422);

                    if (!Uri.TryCreate(request.LongUrl, UriKind.Absolute, out var longUrl))
                        return Results.Problem(detail: "Long url invalid", statusCode: 422);

                    var sizeShortUrl = configuration.GetValue<int>("DefaultSizeShortUrl");
                    string shortUrl;

                    do
                    {
                        shortUrl = GenerateShortUrl(sizeShortUrl);
                    } while (await urls.FirstOrDefault(u => u.ShortUrl == shortUrl).ExecuteAsync() != null);

                    var url = new Url(longUrl.ToString(), shortUrl);

                    await urls.Insert(url).ExecuteAsync().ConfigureAwait(false);

                    return Results.Ok(new CreateShortUrlResponse(url.LongUrl, url.ShortUrl));
                })
            .WithName("AdminUrlShortener")
            .WithOpenApi();

        return app;
    }

    private static string GenerateShortUrl(int length)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        var hashString = Convert.ToBase64String(hashBytes).Replace("=", "").Replace("/", "").Replace("+", "");

        return hashString[..Math.Min(length, hashString.Length)];
    }
}