using bijjamVilla__Dotnet_core___web_API_.Data;
using bijjamVilla__Dotnet_core___web_API_.DTO;
using bijjamVilla__Dotnet_core___web_API_.Model;
using bijjamVilla__Dotnet_core___web_API_.Services;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper( o =>
{
    o.CreateMap<villaCreateDTO, villa> ();        //.CreateMap <source , destination> ()
    o.CreateMap<villaUpdateDTO, villa> ();
    o.CreateMap<villa, villaDTO> ();
    o.CreateMap<User, UserDTO>();

});
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();
//await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
