using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var isCors = CorsRegister(builder);

var app = builder.Build();

if (isCors)
{
    app.UseCors("KortrosCorsPolicy");
}

StaticFilesRegister(builder, app);

app.Run();

static bool CorsRegister(WebApplicationBuilder builder)
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var isCorsFromConfig = !string.IsNullOrEmpty(env) && (env == Environments.Production || env == Environments.Development);

    string[] webUrls = [];

    if (isCorsFromConfig || string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WebUrl")))
    {
        webUrls = builder.Configuration.GetSection("WebUrl").Get<string[]>();
        webUrls ??= [builder.Configuration.GetValue<string>("WebUrl")];
    }
    else
    {
        webUrls = [Environment.GetEnvironmentVariable("WebUrl")];
    }

    if (webUrls.Length == 0)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "InsuranceGoSmokeCorsPolicy",
                builder =>
                {
                    builder.WithOrigins(webUrls)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                });
        });
        return true;
    }

    return false;
}

static void StaticFilesRegister(WebApplicationBuilder builder, WebApplication application)
{
    var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
    application.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
               Path.Combine(builder.Environment.ContentRootPath, "Templates")),
        RequestPath = "/Templates",
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.Append(
                 "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
        }
    });
}