
using Api.Minimals;
using Application.Entities;
using Scalar.AspNetCore;

namespace Api
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // specifies how much space can an image be allocated in memory that is processed by NetVips.
            // so this env means : 5 megabytes if the image is bigger, then it uses temp files instead
            GlobalSetups.Init();
            Environment.SetEnvironmentVariable("VIPS_DISC_THRESHOLD", "5m");

            config.AddEnvironmentVariables();
            config.AddUserSecrets("7f342e59-c0e1-4ef5-9bd1-126a96fa7a5b");

            builder.Services.AddApiServices(config, builder.Environment)
                            .AddApplicationServices(config)
                            .AddInfrastructureServices(config, builder.Environment);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                ApplicationDbContextInitialiser initialiser = new(scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContextInitialiser>>(),
                    scope.ServiceProvider.GetRequiredService<AppDbContext>(),
                    scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>(),
                    scope.ServiceProvider.GetRequiredService<RoleManager<Role>>());

                await initialiser.InitialiseAndSeedData();
            }



            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
                app.MapOpenApi();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseStatusCodePages();
                //app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "home.html" }
            });
            app.UseStaticFiles();
            app.MapFallbackToFile("home.html");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.Use(async (HttpContext context, RequestDelegate next) =>
            {
                if (!Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var _))
                {
                    var guestId = context.Request.Cookies["guest_id"];

                    if (!Guid.TryParse(guestId, out var _))
                    {
                        guestId = Guid.CreateVersion7().ToString();

                        context.Response.Cookies.Append("guest_id", guestId,
                        new CookieOptions
                        {
                            IsEssential = true,
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.Now.AddDays(7),
                            Secure = true,
                        });
                        context.Items["guest_id"] = guestId;
                    }

                }
                await next(context);

            });

            app.MapControllers();

            app.MapStripeWebhookEndpoints();

            app.MapGroup("/api/auth").
                MapIdentityApi<AppUser>();

            app.MapPost("api/auth/logout", async (
                SignInManager<AppUser> signInManager,
                [FromBody] object empty) =>
            {
                if (empty != null)
                {
                    await signInManager.SignOutAsync();
                    return Results.Ok();
                }

                return Results.Unauthorized();
            })
            .RequireAuthorization();

            await app.RunAsync();
        }
    }
}
