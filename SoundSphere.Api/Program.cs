using Microsoft.EntityFrameworkCore;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Database.Repositories;
using System.Text.Json.Serialization;
using SoundSphere.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SoundSphereDbContext>(options => options.UseSqlServer(
    string.Format(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Missing connection string"),
                  Environment.GetEnvironmentVariable("SQLSERVER_HOST"),
                  Environment.GetEnvironmentVariable("SQLSERVER_USERID"),
                  Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD")),
    sqlOptions => sqlOptions.MigrationsAssembly("SoundSphere.Api")));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(SoundSphere.Core.Mappings.AutoMapperProfile).Assembly);
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IAuthorityRepository, AuthorityRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAuthorityService, AuthorityService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();