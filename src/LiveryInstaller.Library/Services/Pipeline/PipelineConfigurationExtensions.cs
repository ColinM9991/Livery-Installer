using LiveryInstaller.Library.Services.Pipeline.Steps;
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
            services.AddTransient<LiveryExtractionStep>();
            services.AddTransient<LiveryInstallationStep>();
            services.AddTransient<LiveryConfigurationStep>();
            services.AddTransient<VariantInstallConfigurationStep>();

            services.AddTransient<VariantUninstallConfigurationStep>();
            services.AddTransient<LiveryConfigurationUninstallStep>();
            services.AddTransient<LiveryUninstallationStep>();

            services.AddPipeline<LiveryInstallationContext>(cfg =>
                {
                    cfg
                        .AddStep<LiveryExtractionStep>()
                        .AddStep<LiveryInstallationStep>()
                        .AddStep<LiveryConfigurationStep>()
                        .AddStep<VariantInstallConfigurationStep>();
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