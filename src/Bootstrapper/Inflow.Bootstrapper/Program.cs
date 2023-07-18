using Inflow.Shared.Infrastructure;
using Inflow.Shared.Infrastructure.Contracts;
using Inflow.Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
var modules = ModuleLoader.LoadModules(assemblies).ToList();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddModularInfrastructure(assemblies);
builder.Host.ConfigureModules();

modules.ForEach(m => m.Register(builder.Services));

var app = builder.Build();

app.Logger.LogInformation($"Modules: {string.Join(", ", modules.Select(x => x.Name))}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseModularInfrastructure();

modules.ForEach(m => m.Use(app));

app.ValidateContracts(assemblies);

app.MapControllers();


app.Run();
