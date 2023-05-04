using Inflow.Shared.Infrastructure;
using Inflow.Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
var modules = ModuleLoader.LoadModules(assemblies);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddModularInfrastructure(assemblies);
foreach (var module in modules)
{
    module.Register(builder.Services);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

foreach (var module in modules)
{
    module.Use(app);
}

app.MapControllers();


app.Run();