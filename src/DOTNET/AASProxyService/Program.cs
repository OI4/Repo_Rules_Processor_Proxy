using System.Text.Json;
using System.Text.Json.Serialization;
using AasDemoapp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerDocument(config =>
{
    config.DocumentName = "Internal API";
    config.Title = "Internal API";
    config.Description = "Internal API for interaction with frontend";
    config.PostProcess = document =>
    {
        document.Info.Version = "v1.0";
        document.Info.Contact = new NSwag.OpenApiContact
        {
            Name = "Meta Level Software AG"
        };
    };
    config.SchemaSettings.DefaultReferenceTypeNullHandling = NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
});


// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});



//  builder.Services.AddScoped<AasDemoappContext>(provider => provider.GetService<AasDemoappContext>());




var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();


// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");


app.Run();