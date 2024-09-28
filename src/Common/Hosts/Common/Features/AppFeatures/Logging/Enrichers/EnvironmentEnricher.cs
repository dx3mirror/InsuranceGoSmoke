using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Logging.Enrichers
{
    /// <summary>
    /// Обогащения данными окружения.
    /// </summary>
    public sealed class EnvironmentEnricher : ILogEventEnricher
    {
        private const string ApplicationNameProperty = "ApplicationName";
        private const string EnvironmentNameProperty = "EnvironmentName";
        private const string ApplicationVersionProperty = "ApplicationVersion";
        private const string MachineNameProperty = "MachineName";
        private const string HostNameProperty = "HostName";
        private const string StandNameProperty = "StandName";

        private static readonly string Unknown = "Unknown";

        private readonly ConcurrentDictionary<string, LogEventProperty> _propertyCache;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment? _environment;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="environment">Окружение.</param>
        public EnvironmentEnricher(IConfiguration configuration, IHostEnvironment? environment)
        {
            _configuration = configuration;
            _environment = environment;
            _propertyCache = new ConcurrentDictionary<string, LogEventProperty>();
        }

        /// <inheritdoc/>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_propertyCache.GetOrAdd(
                ApplicationNameProperty, key =>
                {
                    var applicationName = _configuration.GetValue<string>(key);
                    if (string.IsNullOrEmpty(applicationName))
                    {
                        applicationName = _environment?.ApplicationName ?? Assembly.GetEntryAssembly()?.GetName().Name;
                    }

                    var property =  propertyFactory.CreateProperty(key, applicationName);
                    return property;
                }));

            logEvent.AddPropertyIfAbsent(_propertyCache.GetOrAdd(
                EnvironmentNameProperty, key =>
                {
                    var environmentName = GetEnvironmentName();
                    return propertyFactory.CreateProperty(key, environmentName);
                }));

            logEvent.AddPropertyIfAbsent(_propertyCache.GetOrAdd(
                ApplicationVersionProperty, key =>
                {
                    var appVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? Unknown;
                    return propertyFactory.CreateProperty(key, appVersion);
                }));

            logEvent.AddPropertyIfAbsent(_propertyCache.GetOrAdd(
                MachineNameProperty, key =>
                {
                    var machineName = Environment.MachineName;
                    return propertyFactory.CreateProperty(key, machineName);
                }));

            logEvent.AddPropertyIfAbsent(_propertyCache.GetOrAdd(
                HostNameProperty, key =>
                {
                    try
                    {
                        var hostName = Dns.GetHostName();
                        return propertyFactory.CreateProperty(key, hostName);
                    }
                    catch (Exception)
                    {
                        return propertyFactory.CreateProperty(key, Unknown);
                    }
                }));

            logEvent.AddPropertyIfAbsent(_propertyCache.GetOrAdd(StandNameProperty, key =>
            {
                var applicationName = _configuration.GetValue<string>(key);

                if (string.IsNullOrEmpty(applicationName))
                {
                    applicationName = GetEnvironmentName();
                }

                return propertyFactory.CreateProperty(key, applicationName);
            }));
        }

        private string GetEnvironmentName()
        {
            return _environment?.EnvironmentName ?? Unknown;
        }
    }
}
