using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ScooterRent.API.Entities;
using ScooterRent.API.Infrastructure.Interfaces;
using ScooterRent.API.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
var connectionString = configuration.GetConnectionString("MongoConnectionString");
var client = new MongoClient(connectionString);
var database = client.GetDatabase("ScooterRentDB");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<User>, Repository<User>>(provider => new Repository<User>(database, "user"));
builder.Services.AddScoped<IRepository<Scooter>, Repository<Scooter>>(provider => new Repository<Scooter>(database, "scooter"));
builder.Services.AddScoped<IRepository<Remainder>, Repository<Remainder>>(provider => new Repository<Remainder>(database, "remainder"));
builder.Services.AddScoped<IRepository<TransactionHistory>, Repository<TransactionHistory>>(provider => new Repository<TransactionHistory>(database, "transactionHistory"));
builder.Services.AddScoped<IRepository<RentDetail>, Repository<RentDetail>>(provider => new Repository<RentDetail>(database, "rentDetail"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseHsts();
//}
app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
