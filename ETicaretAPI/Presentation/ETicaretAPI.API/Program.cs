using ETicaretAPI.Application;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using ETicaretAPI.Persistence.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin",options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateAudience = false, //oluşturulacak token deperini kimlerin/hangi originlerin/sitelerin kullanıcı belirlediğimiz değerdir
        ValidateIssuer = true, //oluşturulacak token değerini kimin dağırrınığını ifade edeceğimiz alandır ->www.myapi.com
        ValidateLifetime=true, //oluşturulan token değerinin süresini kontrol edecek olan doğrulamadırç
        ValidateIssuerSigningKey=true, //üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden sucirty key verisinin doğrulanmasıdır.

        ValidAudience = builder.Configuration["Token:Audince"],
            ValidIssuer= builder.Configuration["Token:Issuer"],
        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
 
    };
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetRequiredService<ETicaretAPIDbContext>().RunMigration();

app.Run();

