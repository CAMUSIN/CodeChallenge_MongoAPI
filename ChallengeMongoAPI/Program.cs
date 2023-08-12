using ChallengeMongoAPI.Models;
using ChallengeMongoAPI.Models.Abstractions;
using ChallengeMongoAPI.Services.Abstractions;
using ChallengeMongoAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChallengeMongoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("MongoDBSettings"));

            builder.Services.AddSingleton<IDatabaseSettings>(
            sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            builder.Services.AddSingleton<IMongoClient>(s =>
                new MongoClient(builder.Configuration.GetValue<string>("MongoDBSettings:ConnectionString")));

            builder.Services.AddScoped<IForecastLocalService, ForecastLocalService>();
            builder.Services.AddScoped<IForecastRemoteService, ForecastRemoteService>();

            builder.Services.AddControllers();

            builder.Services.AddHttpClient("forecastClient", client => {
                client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
            });

            builder.Services.AddHttpClient("geoCodingClient", client => {
                client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/v1/");
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}