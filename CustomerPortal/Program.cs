using CustomerPortal.Data;
using CustomerPortal.Data.Repository;
using CustomerPortal.Services;
using CustomerPortal.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Get Configuration Strings
var dbConnectionString = builder.Configuration.GetConnectionString("dbConnectionString")
                         ?? throw new InvalidOperationException("Missing ConnectionStrings:dbConnectionString.");
var webApiConnectionString = builder.Configuration.GetConnectionString("webApiConnectionString");

//initialize database
builder.Services.AddDbContext<McbaContext>(opt =>
    opt.UseSqlServer(dbConnectionString));

//Register service
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
    {
        o.IdleTimeout = TimeSpan.FromMinutes(30);
        o.Cookie.HttpOnly = true;
        o.Cookie.IsEssential = true;
    }
);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<McbaContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); 

app.UseSession();
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
