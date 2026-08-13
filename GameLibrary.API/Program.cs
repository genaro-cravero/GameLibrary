using GameLibrary;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

namespace GameLibrary.API;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IGameRepository, SqlGameRepository>();
        builder.Services.AddSingleton<GameLibraryService>();

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.MapControllers();

        app.Run();
    }
}