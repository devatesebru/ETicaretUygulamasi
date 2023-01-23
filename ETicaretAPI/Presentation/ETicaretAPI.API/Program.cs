using ETicaretAPI.Persistence;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("PostgreSQL"));
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:58722", "https://localhost:58722").AllowAnyHeader().AllowAnyMethod()));
// yukarıdakinin aynısı 
//ServiceRegistration.AddPersistenceServices(builder.Services, builder.Configuration.GetConnectionString("PostgreSQL"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
