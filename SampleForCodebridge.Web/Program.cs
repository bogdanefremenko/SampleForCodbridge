using Microsoft.EntityFrameworkCore;
using SampleForBridgecode.Business;
using SampleForCodebridge.Data;
using SampleForCodebridge.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("db")));

builder.Services.AddBusiness();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();
app.Run();