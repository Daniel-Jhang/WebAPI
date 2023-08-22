using LabWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace LabWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Set Configuration
            builder.Host.ConfigureAppConfiguration((context, configure) =>
            {
                var environment = context.HostingEnvironment;
                configure.SetBasePath(environment.ContentRootPath)
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: $"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
                .UseSerilog((hostContext, logConfigure) =>
                logConfigure.ReadFrom.Configuration(hostContext.Configuration).Enrich.FromLogContext()
            );

            var configure = builder.Configuration;

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configure).CreateLogger();
            #endregion

            #region Set DataBase Connection
            // EF Core
            if (!int.TryParse(configure["SqlCommandTimeout"], out int commandTimeout))
            {
                commandTimeout = 300;
            }

            builder.Services.AddDbContext<LabContext>(options =>
            {
                switch (configure["Provider"])
                {
                    case "MSSQL":
                        options.UseSqlServer(configure.GetConnectionString("MSSQL"), optionsBuilder =>
                        {
                            optionsBuilder.CommandTimeout(commandTimeout);
                            optionsBuilder.EnableRetryOnFailure();
                        });
                        break;

                    case "Oracle":
                        options.UseOracle(configure.GetConnectionString("Oracle"), optionsBuilder =>
                        {
                            optionsBuilder.CommandTimeout(commandTimeout);
                        });
                        break;

                    // 可以新增其他資料庫提供者的處理邏輯

                    default:
                        throw new NotSupportedException($"Provider {configure["Provider"]} is not supported.");
                }
            }, ServiceLifetime.Scoped);
            #endregion

            // Add services to the container.

            builder.Services.AddControllers();
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