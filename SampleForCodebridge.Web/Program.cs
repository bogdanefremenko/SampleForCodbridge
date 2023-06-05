using Microsoft.EntityFrameworkCore;
using SampleForBridgecode.Business;
using SampleForCodebridge.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("db")));

builder.Services.AddBusiness();

var app = builder.Build();

app.MapControllers();
app.Run();