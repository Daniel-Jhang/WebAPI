try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Configure
    builder.Host.ConfigureAppConfiguration((hostContext, config) =>
    {
        var environment = hostContext.HostingEnvironment;
        config.SetBasePath(environment.ContentRootPath)
        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(path: $"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    })
        .UseSerilog((hostingContext, loggerConfig) =>
         loggerConfig.ReadFrom.Configuration(hostingContext.Configuration).Enrich.FromLogContext()
         );

    var config = builder.Configuration;

    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
    #endregion

    #region DB
    builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(config.GetConnectionString("NorthwindConnection")));
    #endregion

    // Add services to the container.

    builder.Services.AddScoped<IProductAccessService, ProductAccessService>();
    builder.Services.AddScoped<IProductService, ProductService>();

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
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}