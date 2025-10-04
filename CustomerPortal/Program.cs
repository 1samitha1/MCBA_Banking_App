using System.Globalization;
using CustomerPortal.Data;
using CustomerPortal.Data.Repository;
using CustomerPortal.Data.Repository.Impl;
using CustomerPortal.Services;
using CustomerPortal.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var au = new CultureInfo("en-AU");
CultureInfo.DefaultThreadCurrentCulture = au;
CultureInfo.DefaultThreadCurrentUICulture = au;
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

//register webservice
builder.Services.AddTransient<WebService>();

//Register service
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
    {
        o.Cookie.Name = "CustomerPortal.Session";
        o.IdleTimeout = TimeSpan.FromMinutes(30);
        o.Cookie.HttpOnly = true;
        o.Cookie.IsEssential = true;
    }
);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBillPayService, BillPayService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IBillPayRepository, BillPayRepository>();
builder.Services.AddScoped<IPayeeRepository, PayeeRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<McbaContext>();
    db.Database.Migrate();
    var webService = scope.ServiceProvider.GetRequiredService<WebService>();
    await webService.HandleWebRequest();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); 

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}")
    .WithStaticAssets();

app.Run();
