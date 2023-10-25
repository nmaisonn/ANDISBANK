using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(options => {
    options.RejectionStatusCode = 429;
    options.AddFixedWindowLimiter("Fixed window", options => {
        options.AutoReplenishment = true;
        options.PermitLimit = 1000;
        options.Window = TimeSpan.FromMinutes(1);
    });
    options.AddSlidingWindowLimiter("Sliding window", options => {
        options.AutoReplenishment = true;
        options.PermitLimit = 100;
        options.SegmentsPerWindow = 10;
        options.QueueLimit = 1000;
    });
});
var app = builder.Build(); 
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
