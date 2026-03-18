using Microsoft.EntityFrameworkCore;
using HotelReservasAPI.Data;
using HotelReservasAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<CitaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();
app.MapControllers();

app.Run();