using Azure.Identity;
using CustomEmailSender.Services;
using Microsoft.Azure.Cosmos;

namespace CustomEmailSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<CosmosClient>(options =>
            {
                var configuration = builder.Configuration;
                return new CosmosClient(configuration["CosmosDb:Account"], configuration["CosmosDb:authKey"]);
            });
            builder.Services.AddSingleton<ICosmosService, CosmosService>(); 
            builder.Services.AddSingleton<IEmailService, EmailService>();

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsProduction())
            {
                var keyVaultUrl = builder.Configuration["KeyVault:Url"];
                if (!string.IsNullOrEmpty(keyVaultUrl))
                {
                    builder.Configuration.AddAzureKeyVault(
                        new Uri(keyVaultUrl),
                        new DefaultAzureCredential());
                }
            }

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}