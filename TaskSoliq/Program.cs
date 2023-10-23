using Microsoft.EntityFrameworkCore;
using TaskSoliq.Application;
using TaskSoliq.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserServices, UserServices>();

if (builder.Environment.IsDevelopment())
{
    /* Update-Database */
    builder.Services.AddDbContext<TurniketDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("StagingConnection") ?? throw new InvalidOperationException("Connection string 'StagingConnection' not found.")));
}
else
{
    /* Update-Database -Args '--environment Production' */

    builder.Services.AddDbContext<TurniketDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionConnection") ?? throw new InvalidOperationException("Connection string 'ProductionConnection' not found.")));
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();
