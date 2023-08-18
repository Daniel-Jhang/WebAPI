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