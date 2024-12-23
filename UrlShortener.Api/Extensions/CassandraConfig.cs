using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using UrlShortener.Api.Configs;
using UrlShortener.Api.Models;
using ISession = Cassandra.ISession;

namespace UrlShortener.Api.Extensions;

public static class CassandraConfig
{
    public static void AddCassandraConfig(this IServiceCollection services, CassandraConfiguration cassandraConfiguration)
    {
        services.AddSingleton<ISession>(serviceProvider =>
        {
            var cluster = serviceProvider.GetRequiredService<Cluster>();

            var session = cluster.Connect(cassandraConfiguration.Keyspace);

            return session;
        });

        services.AddScoped<Table<Url>>(serviceProvider =>
        {
            var session = serviceProvider.GetRequiredService<ISession>();

            return new Table<Url>(session);
        });
    }
}