using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SoundSphereDbContext>(options => options.UseSqlServer(
    string.Format(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Missing connection string"),
                  Environment.GetEnvironmentVariable("SQLSERVER_HOST"),
                  Environment.GetEnvironmentVariable("SQLSERVER_USERID"),
                  Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD")),
    sqlOptions => sqlOptions.MigrationsAssembly("SoundSphere.Api")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();