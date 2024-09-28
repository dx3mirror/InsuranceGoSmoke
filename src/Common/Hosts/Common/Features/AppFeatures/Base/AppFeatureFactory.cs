using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base
{
    /// <summary>
    /// Фабрика функциональностей.
    /// </summary>
    public static class AppFeatureFactory
    {
        private const string FeaturesSectionName = "Features";

        /// <summary>
        /// Возвращает список функциональностей.
        /// </summary>
        /// <param name="configuration">Настройки.</param>
        /// <param name="featureAssemblies">Сборки, где находятся функциональности.</param>
        /// <returns>Список функциональностей.</returns>
        public static IReadOnlyCollection<IAppFeature> GetAppFeatures(IConfiguration configuration, Assembly[] featureAssemblies)
        {
            var featuresSection = configuration.GetSection(FeaturesSectionName);
            if (!featuresSection.Exists())
            {
                return [];
            }

            var features = new List<IAppFeature>();
            var featuresOptions = new AppFeaturesOptions();

            configuration.Bind(featuresOptions);

            var appFeatureType = typeof(IAppFeature);
            var assemblyFeatures = featureAssemblies
                                    .SelectMany(x => 
                                        x.GetTypes()
                                         .Where(t => !t.IsAbstract && !t.IsInterface && appFeatureType.IsAssignableFrom(t))
                                         .ToHashSet())
                                    .ToArray();

            foreach (var (featureName, featureOptions) in featuresOptions.Features)
            {
                if (featureOptions.Disabled)
                {
                    continue;
                }

                var featureType = Array.Find(assemblyFeatures, t => t.Name == $"{featureName}Feature");
                if (featureType == null)
                {
                    continue;
                }

                var feature = Activator.CreateInstance(featureType) as IAppFeature;
                feature!.Init(new AppFeatureInitRequest
                {
                    Name = featureName,
                    Order = featureOptions.Order,
                    Configuration = configuration,
                    OptionSection = configuration.GetSection(FeaturesSectionName).GetSection(featureName),
                    AdditionalAssemblies = featureAssemblies
                });
                features.Add(feature);
            }

            return features;
        }
    }
}
