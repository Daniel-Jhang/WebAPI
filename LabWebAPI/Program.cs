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
                commandTimeout = 150;
            }

            builder.Services.AddDbContext<LabContext>(options =>
            {
                switch (configure["Provider"])
                {
                    case "MSSQL":
                        options.UseSqlServer(configure.GetConnectionString("MSSQL"), sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.CommandTimeout(commandTimeout);
                            // ����A��s
                            //sqlOptions.EnableRetryOnFailure(
                            //    maxRetryCount: 10,
                            //    maxRetryDelay: TimeSpan.FromSeconds(30),
                            //    errorNumbersToAdd: null);
                            //sqlOptions.ExecutionStrategy(CreateExecutionStrategy);
                        });
                        break;

                    case "Oracle":
                        options.UseOracle(configure.GetConnectionString("Oracle"), optionsBuilder =>
                        {
                            // �b�o�̳]�w Oracle �S�w���ﶵ
                            optionsBuilder.CommandTimeout(commandTimeout);
                        });
                        break;

                    // �i�H�s�W��L��Ʈw���Ѫ̪��B�z�޿�

                    default:
                        throw new NotSupportedException($"Provider {configure["Provider"]} is not supported.");
                }
            }, ServiceLifetime.Scoped);
            #endregion

            #region Add services to the container
            builder.Services.AddScoped<ITodoListDao, TodoListDao>();
            builder.Services.AddScoped<ITodoListService, TodoListService>();
            #endregion

            #region Set Cross-Origin Resource Sharing(CORS)
            var enableCorsHandling = configure.GetValue<bool>("EnableCorsHandling");
            var originsURLs = configure.GetSection("CorsConfigurations").Get<string[]>();

            if (enableCorsHandling && originsURLs != null)
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: "MyAllowSpecificOrigins", builder =>
                    {
                        builder.WithOrigins(originsURLs)
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
                });
            }
            #endregion

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

            if (enableCorsHandling)
            {
                app.UseCors("MyAllowSpecificOrigins"); // �b�o�̥[�J Cross-Origin Resource
            }

            app.MapControllers();

            app.Run();
        }
    }
}