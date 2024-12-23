namespace UrlShortener.Api.Models;

public class Url
{
    public Url()
    {
    }

    public Url(string longUrl, string shortUrl)
    {
        Id = Guid.NewGuid();
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; set; }
    public string LongUrl { get; set; } = null!;
    public string ShortUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}