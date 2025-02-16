using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Filters;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options=>
{
    options.Filters.Add<logActivityFilter>();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(builder=> builder.UseSqlServer("server=.;database=Products;user id=sa;password=sa123456; trust server certificate=true"));
var app = builder.Build(); 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<profilingMiddleware>();

app.MapControllers();

app.Run();
