using Cassandra;
using Cassandra.Mapping;
using UrlShortener.Api.Configs;
using UrlShortener.Api.Models;
using ISession = Cassandra.ISession;

namespace UrlShortener.Api.Extensions;

public static class InitializeConfig
{
    public static void AddInitializeConfig(this IServiceCollection services,
        CassandraConfiguration cassandraConfiguration)
    {
        var cluster = Cluster.Builder()
            .AddContactPoints(cassandraConfiguration.Hosts)
            .WithPort(cassandraConfiguration.Port)
            .WithCredentials(cassandraConfiguration.User, cassandraConfiguration.Password)
            .Build();

        services.AddSingleton<Cluster>(sp => cluster);

        var session = cluster.Connect();

        session.Execute(new SimpleStatement(
            $"CREATE KEYSPACE IF NOT EXISTS {cassandraConfiguration.Keyspace} with replication = {{'class': 'SimpleStrategy', 'replication_factor': 1}};"));
        session.Execute(new SimpleStatement($"USE {cassandraConfiguration.Keyspace}"));
        session.Execute(new SimpleStatement($"""
                                             CREATE TABLE IF NOT EXISTS urls 
                                             (
                                                 id         UUID PRIMARY KEY,
                                                 long_url   TEXT,
                                                 short_url  TEXT,
                                                 created_at TIMESTAMP
                                             );
                                             """));
        session.Execute(new SimpleStatement($"CREATE INDEX IF NOT EXISTS urls_short_url_idx ON urls(short_url);"));
        
        MappingConfiguration.Global.Define(
            new Map<Url>()
                .TableName("urls")
                .PartitionKey(u => u.Id)
                .Column(u => u.Id, cm => cm.WithName("id"))
                .Column(u => u.LongUrl, cm => cm.WithName("long_url"))
                .Column(u => u.ShortUrl, cm => cm.WithName("short_url"))
                .Column(u => u.CreatedAt, cm => cm.WithName("created_at")));
    }
}