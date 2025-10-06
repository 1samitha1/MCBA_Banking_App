using AdminApi.Data.DataManager;
using AdminApi.Data.Repository;
using CustomerPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<McbaContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("CustomerMcbaContext")
             ?? throw new InvalidOperationException("Missing ConnectionStrings:dbConnectionString.");
    options.UseSqlServer(cs);
});
builder.Services.AddScoped<IPayeeRepository, PayeeManager>();
builder.Services.AddScoped<IBillPayRepository, BillPayManager>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
