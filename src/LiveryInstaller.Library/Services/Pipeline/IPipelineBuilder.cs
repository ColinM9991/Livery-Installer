namespace LiveryInstaller.Library.Services.Pipeline;

/// <summary>
/// Provides a builder used to build operation pipelines.
/// </summary>
/// <typeparam name="TContext"></typeparam>
internal interface IPipelineBuilder<TContext>
{
    /// <summary>
    /// Adds a step to the pipeline.
    /// </summary>
    /// <typeparam name="TStep">The type of the step to add.</typeparam>
    /// <returns></returns>
    IPipelineBuilder<TContext> AddStep<TStep>() where TStep : class, IPipelineStep<TContext>;
    
    /// <summary>
    /// Adds a behaviour to the pipeline.
    /// </summary>
    /// <typeparam name="TBehaviour"></typeparam>
    /// <returns></returns>
    IPipelineBuilder<TContext> AddBehaviour<TBehaviour>() where TBehaviour : class, IPipelineBehaviour<TContext>;
    
    /// <summary>
    /// Builds the pipeline with the added steps and behaviours.
    /// </summary>
    /// <returns><see cref="IPipeline{TContext}"/>.</returns>
    IPipeline<TContext> Build();
}