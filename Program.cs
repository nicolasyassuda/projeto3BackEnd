using Microsoft.OpenApi.Models;
using APIMOVIES.Swagger;
using APIMOVIES.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? mongoURI = Environment.GetEnvironmentVariable("MONGODB_URI");
string? mongoDBName = Environment.GetEnvironmentVariable("MONGODB_DBNAME");
var mongoDBService = new MongoDBService();

builder.Services.AddSingleton<MongoDBService>(mongoDBService);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
options.SwaggerEndpoint("/swagger/v1/swagger.json",
"Documentation v1"));

//}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
