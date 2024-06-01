using Blog.Application.Extensions;
using Blog.Presentation.Extensions;
using Blog.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServiceCollectionConfigurations(builder.Configuration);
builder.Services.AddInfrastructureServiceCollectionsConfigurations(builder.Configuration);
builder.Services.AddApplicationServiceCollectionConfigurations(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.AddInfrastructureApplicationConfigurations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.AddPersistentApplicationBuilderConfigurations();

app.MapControllers();

app.Run();

public partial class Program
{

}