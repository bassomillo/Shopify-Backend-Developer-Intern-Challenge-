
using Microsoft.OpenApi.Models;
using ShopifyChallengeBackEndApi.Models;
using ShopifyChallengeBackEndApi.Services;
using System;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ShopifyChallengeDataBaseSettings>(
    builder.Configuration.GetSection("ShopifyChallengeDataBase"));
builder.Services.AddSingleton<InventoryService>();
builder.Services.AddSingleton<RecycleService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "policy",
        builder =>
        {
            builder.WithOrigins("https://localhost:7284")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Shopify Challenge BackEnd API",
        Description = "An ASP.NET Core Web API with MongoDB",
        TermsOfService = new Uri("https://cloud.mongodb.com/v2/62847525b133a0137ced123f#clusters"),
        
        License = new OpenApiLicense
        {
            Name = "Document",
            Url = new Uri("https://docs.google.com/document/d/1sFRlpRc_x9s1x9RgBW1YOFIsTJV6en-TekG9I6Ff5fw/edit?usp=sharing")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

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

//mongoDB password:  ShopifyChallenge
