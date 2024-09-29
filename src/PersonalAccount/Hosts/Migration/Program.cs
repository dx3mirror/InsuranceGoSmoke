using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.PersonalAccount.Hosts.Migrations;
using InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var app = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = hostContext.Configuration.GetConnectionString("PersonalAccountDb");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        }

        services.AddDbContext<PersonalAccountsDbContext>(dbContextBuilder =>
            dbContextBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

        services.AddSingleton<Startup>();
    }).Build();

var migration = app.Services.GetService<Startup>()
                    ?? throw new NotFoundException("Не удалось получить контекст доступа к базе данных");

using var cancellationTokenSource = new CancellationTokenSource();

await migration.StartMigrationsAsync(cancellationTokenSource.Token);