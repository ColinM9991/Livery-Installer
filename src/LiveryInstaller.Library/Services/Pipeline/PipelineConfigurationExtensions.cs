using LiveryInstaller.Library.Services.Pipeline.Behaviours;
using LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;
using LiveryInstaller.Library.Services.Pipeline.Steps.UninstallLiverySteps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.Library.Services.Pipeline;

public static class PipelineConfigurationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPipelineOperations()
        {
            services.Scan(x =>
                x.FromAssemblyOf<LiveryPostInstallationStep>()
                    .AddClasses(
                        classes => classes.AssignableToAny(typeof(IPipelineStep<>), typeof(IPipelineBehaviour<>)),
                        publicOnly: false)
                    .AsSelf()
                    .WithTransientLifetime());

            services.AddPipeline<LiveryInstallationContext>(cfg =>
                {
                    cfg
                        .AddStep<LiveryExtractionStep>()
                        .AddStep<LiveryInstallationStep>()
                        .AddStep<LiveryConfigurationStep>()
                        .AddStep<VariantInstallConfigurationStep>()
                        .AddStep<LiveryPostInstallationStep>()
                        .AddBehaviour<LoggingBehaviour<LiveryInstallationContext>>();
                })
                .AddPipeline<LiveryUninstallContext>(cfg =>
                {
                    cfg
                        .AddStep<VariantUninstallConfigurationStep>()
                        .AddStep<LiveryConfigurationUninstallStep>()
                        .AddStep<LiveryUninstallationStep>();
                });

            return services;
        }

        private IServiceCollection AddPipeline<TContext>(
            Action<PipelineBuilder<TContext>> configure)
        {
            services.AddTransient(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<PipelineBuilder<TContext>>>();

                var builder = new PipelineBuilder<TContext>(sp, logger);

                configure(builder);

                return builder.Build();
            });

            return services;
        }
    }
}