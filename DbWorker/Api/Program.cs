using Api.Libraries.AddressLibrary.Db;
using Api.Libraries.AddressLibrary.Repositories;

namespace Api;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
        {
            var db =
                builder.Configuration.GetValue<string>("DB_FILE")
                ?? throw new ArgumentException("DB_FILE not set");

            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), db);

            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException($"Database file not found: {dbPath}");
            }

            return new SqliteConnectionFactory($"Data Source={dbPath}");
        });

        builder.Services.AddScoped<IAddressRepository, AddressRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}
