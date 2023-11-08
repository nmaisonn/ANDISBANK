using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



const string serviceName = "enano";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.AddFixedWindowLimiter("Fixed window", options =>
    {
        options.AutoReplenishment = true;
        options.PermitLimit = 1000;
        options.Window = TimeSpan.FromMinutes(1);
    });
    options.AddSlidingWindowLimiter("Sliding window", options =>
    {
        options.QueueLimit = 0;
        options.PermitLimit = 900;
        options.SegmentsPerWindow = 10;
        options.Window = TimeSpan.FromMinutes(1);
    });
    options.AddTokenBucketLimiter("Token bucket", options =>
    {
        options.AutoReplenishment = true;
        options.TokenLimit = 500;
        options.ReplenishmentPeriod = TimeSpan.FromMinutes(0.5);
        options.QueueLimit = 0;
        options.TokensPerPeriod = 250;
    });
    options.AddConcurrencyLimiter("Concurrency", options =>
    {
        options.QueueLimit = 0;
        options.PermitLimit = 10;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(policy => policy
       .Expire(TimeSpan.FromSeconds(10)));

    options.AddPolicy("AlbañilesPolicy", policy => policy
       .Expire(TimeSpan.FromSeconds(10)));
}
);
builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter());
var app = builder.Build();

app.UseOutputCache();
app.UseRateLimiter();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();




app.MapControllers();

app.Run();
