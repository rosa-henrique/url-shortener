namespace UrlShortener.Api.Dto;

public record CreateShortUrlResponse(string LongUrl, string ShortUrl);