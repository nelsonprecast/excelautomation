using AutoMapper;
using Core.Infrastructure;
using Core.Infrastructure.Mapper;
using ExcelAutomation.Data;
using Facade.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
});
var applicationSettings = builder.Configuration.GetSection("SugarCrmConfiguration").Get<ApplicationSettings>();
builder.Services.AddSingleton(applicationSettings);
var connectionString = builder.Configuration.GetConnectionString("ExcelAutomation");

// DB Context
builder.Services.AddDbContext<IDbContext, ExcelAutomationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ExcelAutomationContext>(x => x.UseSqlServer(connectionString));

// Register Repository
builder.Services
    .AddScoped<ExcelAutomation.Service.IPlanElevationTextService, ExcelAutomation.Service.PlanElevationTextService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// Register Services
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IProjectService>()
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Register Facade
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IProjectFacade>()
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Facade")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());


//find mapper configurations provided by other assemblies
var type = typeof(IOrderedMapperProfile);
var mapperConfigurations = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes().Where(x => x.IsClass && !x.IsAbstract))
    .Where(p => type.IsAssignableFrom(p));
//create and sort instances of mapper configurations
var instances = mapperConfigurations
    .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
    .OrderBy(mapperConfiguration => mapperConfiguration.Order);

//create AutoMapper configuration
var config = new MapperConfiguration(cfg =>
{
    foreach (var instance in instances)
    {
        cfg.AddProfile(instance.GetType());
    }

});

//register auto mapper.
AutoMapperConfiguration.Init(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
