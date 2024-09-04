using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PirateTools.Harbor.Endpoints.AskYourChairs;
using PirateTools.Harbor.Models;
using PirateTools.Harbor.Services;
using PirateTools.Harbor.SimpleEndpoints;
using System;
using System.IO;
using System.Text.Json;

namespace PirateTools.Harbor;

public static class Program {
    public static void Main(string[] args) {
        if (!File.Exists("config.json")) {
            File.WriteAllText("config.json", JsonSerializer.Serialize(new Config()));
            Console.WriteLine("Could not find the config.json, wrote an empty one.");
            return;
        }

        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json")) ?? new();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options => {
            options.AddPolicy("default", policy => {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers();

        builder.Services.AddSingleton(new DBService());
        builder.Services.AddSingleton(new SmtpService(config));

        var app = builder.Build();
        //app.UseHttpsRedirection();

        app.UseCors("default");

        app.MapSimple<GetQuestions>("api/AskYourChairs");
        app.MapSimple<AskQuestion>("api/AskYourChairs");
        app.MapSimple<CheckToken>("api/AskYourChairs");
        app.MapSimple<RequestToken>("api/AskYourChairs");

        app.Run();
    }
}