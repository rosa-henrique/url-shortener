using UrlShortener.Api.Configs;
using UrlShortener.Api.Endpoints;
using UrlShortener.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var cassandraConfig = builder.Configuration.GetSection(CassandraConfiguration.SectionConfig).Get<CassandraConfiguration>();

builder.Services.AddInitializeConfig(cassandraConfig);
builder.Services.AddCassandraConfig(cassandraConfig);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAdminUrlEndpoint().UseUrlEndpoint();

app.Run();