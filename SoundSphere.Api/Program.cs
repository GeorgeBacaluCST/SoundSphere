using Microsoft.EntityFrameworkCore;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Core.Services;
using SoundSphere.Database.Context;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Database.Repositories;
using System.Text.Json.Serialization;
using SoundSphere.Infrastructure.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

public class Program
{
    static void Main(string[] args)
    {
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

        builder.Services.AddScoped<IAlbumRepository, AlbumRepository>()
                        .AddScoped<IArtistRepository, ArtistRepository>()
                        .AddScoped<IAuthorityRepository, AuthorityRepository>()
                        .AddScoped<IFeedbackRepository, FeedbackRepository>()
                        .AddScoped<INotificationRepository, NotificationRepository>()
                        .AddScoped<IPlaylistRepository, PlaylistRepository>()
                        .AddScoped<IRoleRepository, RoleRepository>()
                        .AddScoped<ISongRepository, SongRepository>()
                        .AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IAlbumService, AlbumService>()
                        .AddScoped<IArtistService, ArtistService>()
                        .AddScoped<IAuthorityService, AuthorityService>()
                        .AddScoped<IFeedbackService, FeedbackService>()
                        .AddScoped<INotificationService, NotificationService>()
                        .AddScoped<IPlaylistService, PlaylistService>()
                        .AddScoped<IRoleService, RoleService>()
                        .AddScoped<ISongService, SongService>()
                        .AddScoped<IUserService, UserService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SoundSphere API", Description = "This is a sample REST API documentation for a music streaming service.", Version = "1.0" });
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            ExecuteSql(app.Services, Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Scripts", $"{Assembly.GetExecutingAssembly().GetName().Name}.sql"));
        }
        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    static void ExecuteSql(IServiceProvider services, string path)
    {
        var context = services.CreateScope().ServiceProvider.GetRequiredService<SoundSphereDbContext>();
        if (!File.Exists(path)) throw new FileNotFoundException("SQL file not found", path);
        context.Database.ExecuteSqlRaw(File.ReadAllText(path));
    }
}