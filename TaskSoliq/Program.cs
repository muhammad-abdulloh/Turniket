using Microsoft.EntityFrameworkCore;
using TaskSoliq.Application;
using TaskSoliq.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // vaqtinchalik test uchun
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("corsapp");

app.UseStaticFiles();

app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TurniketDbContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
