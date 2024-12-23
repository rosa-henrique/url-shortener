namespace UrlShortener.Api.Configs;

public record CassandraConfiguration(
    IReadOnlyList<string> Hosts,
    int Port,
    string User,
    string Password,
    string Keyspace)
{
    public const string SectionConfig = "CassandraConfig";
}