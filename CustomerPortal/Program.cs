using CustomerPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Get Configuration Strings
var dbConnectionString = builder.Configuration.GetConnectionString("dbConnectionString");
    if (string.IsNullOrWhiteSpace(dbConnectionString)) {
        throw new InvalidOperationException(
            "Missing or empty dbConnectionString. Need to set it in appsettings.json");
    }
var webApiConnectionString = builder.Configuration.GetConnectionString("webApiConnectionString");
    if (string.IsNullOrWhiteSpace(webApiConnectionString)) {
        throw new InvalidOperationException(
            "Missing or empty webApiConnectionString. Need to set it in appsettings.json");
    }

//initialize database
builder.Services.AddDbContext<McbaContext>(opt =>
    opt.UseSqlServer(dbConnectionString));

var app = builder.Build();

//apply migration on startup
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<McbaContext>();
//     db.Database.Migrate();   // creates DB / applies migrations
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    // name: "default",
    // pattern: "{controller=Home}/{action=Index}/{id?}")
    // .WithStaticAssets();

    name: "default",
    pattern: "{controller=Login}/{action=Index}")
    .WithStaticAssets();


app.Run();
