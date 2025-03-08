using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApplication1.Data;
using WebApplication1.Filters;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<ApplicationDbContext>(builder =>
    builder.UseSqlServer("server=.;database=Products;user id=sa;password=sa123456; trust server certificate=true"));

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

app.Run();
