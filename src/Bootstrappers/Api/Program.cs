var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => 
    config.ReadFrom.Configuration(context.Configuration));

//builder.Services.AddCarter(configurator: config =>
//{
//    var catalogModules = typeof(CatalogModule).Assembly.GetTypes()
//    .Where(t=>t.IsAssignableTo(typeof(ICarterModule))).ToArray();

//    config.WithModules(catalogModules);
//});




var catelogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(
    catelogAssembly,
    basketAssembly);

builder.Services.AddMediatRWithAssemblies(
    catelogAssembly,
    basketAssembly);

//builder.Services.AddMediatR(config =>
//{
//    config.RegisterServicesFromAssemblies(catelogAssembly, basketAssembly);
//    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
//    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
//});

//builder.Services.AddValidatorsFromAssemblies([catelogAssembly, basketAssembly]);

builder.Services.AddStackExchangeRedisCache(options =>
{
    //options.Configuration = builder.Configuration.GetValue<string>("Redis");
   
    options.Configuration = builder.Configuration.GetConnectionString("Redis");


});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, catelogAssembly, basketAssembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();

app.UseSerilogRequestLogging();

app.UseExceptionHandler(options => { });

app.UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.UseExceptionHandler(option => { });

app.Run();
