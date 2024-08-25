var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

int limit = builder.Configuration.GetValue<int>("RateLimiting:Limit");
int windowSize = builder.Configuration.GetValue<int>("RateLimiting:WindowSize");

builder.Services.AddSingleton(n => new FixedWindowRateLimiter(limit, TimeSpan.FromSeconds(windowSize)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
