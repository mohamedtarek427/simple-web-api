using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using WebApplication1.Data;
using WebApplication1.Filters;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()  // Log to console
    .WriteTo.Debug()    // Log to Debug output
    .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day) // Log to a file, rotated daily
    .CreateLogger();

// Add Serilog to logging
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<logActivityFilter>();
});

// Add CORS Policy to Allow Everything
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=db15246.public.databaseasp.net,1433; Database=db15246; User Id=db15246; Password=g%8S=7NsoY-4; TrustServerCertificate=True; MultipleActiveResultSets=True;"
   // options.UseSqlServer("server=.;database=Products;user id=sa;password=sa123456; trust server certificate=true"

));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // This makes Swagger available at root URL
    });
}

// Apply CORS Before Other Middleware
app.UseCors("AllowAll");

// Create uploads directory if it doesn't exist
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<profilingMiddleware>();

app.MapControllers();

// Ensure logs are flushed on shutdown
app.Lifetime.ApplicationStopping.Register(Log.CloseAndFlush);

app.Run();
