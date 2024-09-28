using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider
{
    /// <inheritdoc/>
    public class DiagnosticProvider : IDiagnosticProvider
    {
        private readonly ILogger<DiagnosticProvider> _logger;

        private AssemblyName? CurrentAssembly { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        public DiagnosticProvider(ILogger<DiagnosticProvider> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public Activity? StartActivity(Assembly? assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            CurrentAssembly = assembly.GetName();
            var activitySource = new ActivitySource(CurrentAssembly.Name!);
            var activityListener = new ActivityListener
            {
                ShouldListenTo = s => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> activityOptions) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> activityOptions) => ActivitySamplingResult.AllData
            };
            ActivitySource.AddActivityListener(activityListener);
            return activitySource.StartActivity(CurrentAssembly.Name!);
        }

        /// <inheritdoc/>
        public Counter<T>? CreateCounter<T>(string name) where T : struct
        {
            return CreateCounter<T>(name, description: string.Empty);
        }

        /// <inheritdoc/>
        public Counter<T>? CreateCounter<T>(string name, string? description) where T : struct
        {
            if (CurrentAssembly == null)
            {
                _logger.LogWarning("Текущая сборка для диагностики не установлена, вызовите сначала метод {Method}", nameof(StartActivity));
                return null;
            }

            var addedMeter = new Meter(CurrentAssembly.Name!, CurrentAssembly.Version?.ToString());
            var counter = addedMeter.CreateCounter<T>(name, description: description);

            return counter;
        }
    }
}
