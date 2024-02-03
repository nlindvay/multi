using multi.lib.common.interfaces;
using multi.lib.tms1;
using MongoDB.Driver;
using multi.lib.common;
using multi.store.shipment;
using multi.store.client;
using multi.api.shipment;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions<TmsOptions>().Bind(builder.Configuration.GetSection(TmsOptions.Section));
builder.Services.AddSingleton(new MongoClient(builder.Configuration.GetConnectionString("MultiDb")).GetDatabase("MultiDb"));
builder.Services.AddSingleton<IShipmentRepository, ShipmentRepository>();
builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddSingleton<ICurrentClientAccessor, CurrentClientAccessor>();
builder.Services.AddHttpContextAccessor();

// configure the external services to be used for shipments
builder.Services.AddHttpClient<ITmsPlugin, ShipperinoService>(client => client.BaseAddress = new Uri("https://localhost:5001"));
builder.Services.AddHttpClient<ITmsPlugin, ShipCoService>(client => client.BaseAddress = new Uri("https://localhost:5001"));
builder.Services.AddSingleton<ITmsPluginProvider, ShipmentServiceProvider>();

builder.Services.AddLogging();

builder.Services.AddControllers(cfg =>
{
    cfg.Filters.Add<AuthorizationFilter>();
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.SwaggerDoc("v1", new() { Title = "multi.api.shipment", Version = "v1" });
    cfg.AddSecurityDefinition("AccessKey", new OpenApiSecurityScheme
    {
        Name = "AccessKey",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "AccessKey for the client",
        Scheme = "ApiKey"
    });
    cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "AccessKey"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthorization();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
