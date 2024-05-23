using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ss;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddHealthChecks()
    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHealthChecksUI(options =>
    {
        options.AddHealthCheckEndpoint("Healthcheck API", "/healthcheck");

    })
    .AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResultStatusCodes=
    {
        [HealthStatus.Healthy]=StatusCodes.Status200OK,
        [HealthStatus.Degraded]=StatusCodes.Status200OK,
        [HealthStatus.Unhealthy]=StatusCodes.Status503ServiceUnavailable
    }

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthcheck", new()
{
    ResponseWriter =UIResponseWriter.WriteHealthCheckUIResponse

});
app.MapHealthChecksUI(options => options.UIPath = "/dashboard");

app.Run();
