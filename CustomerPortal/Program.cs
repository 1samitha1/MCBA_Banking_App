var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Get Configuration Strings
var dbConnectionString = builder.Configuration.GetConnectionString("dbConnectionString");
var webApiConnectionString = builder.Configuration.GetConnectionString("webApiConnectionString");

var app = builder.Build();

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
