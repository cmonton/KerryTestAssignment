using Api.AddressApiClient;
using Api.Handlers;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var addressApi = builder.Configuration.GetValue<string>("ADDRESS_API") 
            ?? throw new ArgumentException("ADDRESS_API not set");

        // ----------- Add this part to register the generated client -----------
        // Add Kiota handlers to the dependency injection container
        builder.Services.AddKiotaHandlers();

        // Register the factory for the Address client
        builder.Services.AddHttpClient<AddressClientFactory>((sp, client) => {
            // Set the base address and accept header
            // or other settings on the http client
            client.BaseAddress = new Uri(addressApi);
        }).AttachKiotaHandlers(); // Attach the Kiota handlers to the http client, this is to enable all the Kiota features.

        // Register the GitHub client
        builder.Services.AddTransient(sp => sp.GetRequiredService<AddressClientFactory>().GetClient());
        // ----------- Add this part to register the generated client end -------

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

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.Run();
    }
}
