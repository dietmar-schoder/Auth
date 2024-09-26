using Auth.DTO;
using System.Reflection;

namespace Auth.ApiEndpoints;

public static class ApiEndpoints
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/", ServiceAlive);
        app.MapGet("/api", ServiceAlive);
        app.MapPost("/api/login", LoginAsync);

        app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            })
            .UseHttpsRedirection();
        ;
        return app;
    }

    private static ServiceAlive ServiceAlive()
        => new() { Version = Assembly.GetEntryAssembly().GetName().Version.ToString() };

    private async static Task<UserDto> LoginAsync(Login login)
        => await Task.FromResult(new UserDto(Guid.NewGuid(), login.Email, "JWT-" + login.Password));
}
