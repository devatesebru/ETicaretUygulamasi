using ETicaretAPI.Application;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using ETicaretAPI.Persistence.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("PostgreSQL"));
builder.Services.AddInfrastructureServices();
builder.Services.AddStorage<LocalStorage>();
builder.Services.AddApplicationServices();
//builder.Services.AddStorage(StorageType.Local);
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://eticaret.ebruates.net", "https://eticaret.ebruates.net").AllowAnyHeader().AllowAnyMethod()));
// yukarıdakinin aynısı 
//ServiceRegistration.AddPersistenceServices(builder.Services, builder.Configuration.GetConnectionString("PostgreSQL"));
builder.Services.AddControllers(options=> options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).ConfigureApiBehaviorOptions(options=> options.SuppressModelStateInvalidFilter=true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Services.CreateScope().ServiceProvider.GetRequiredService<ETicaretAPIDbContext>().RunMigration();

app.Run();

