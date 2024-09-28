using InsuranceGoSmoke.Common.Consumers.Options;
using InsuranceGoSmoke.Common.Consumers.Services.Builders;
using InsuranceGoSmoke.Common.Consumers.Services.Consumers;
using InsuranceGoSmoke.Common.Consumers.Services.Managers;
using InsuranceGoSmoke.Common.Contracts;
using InsuranceGoSmoke.Notification.Clients;
using InsuranceGoSmoke.Notification.Consumers.Consumers;
using InsuranceGoSmoke.Security.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<HostingService>();
        services.AddConfigurationOptions<KafkaOptions>();

        services.AddTransient<IKafkaMessageConsumerManager, KafkaMessageConsumerManager>();

        services.AddTransient<IKafkaConsumerBuilder, KafkaConsumerBuilder>();

        services.AddSingleton<IKafkaTopicConsumer, EmailNotificationConsumer>();

        services.AddNotificationServiceClient();
    }).Build().Run();