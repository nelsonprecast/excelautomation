using ExcelAutomation.Data;
using ExcelAutomation.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("ExcelAutomation");
builder.Services.AddDbContext<ExcelAutomationContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<IProjectService,ProjectService>();
builder.Services.AddScoped<IPlanElevationReferenceService, PlanElevationReferenceService>();
builder.Services.AddScoped<IPlanElevationTextService, PlanElevationTextService>();

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
