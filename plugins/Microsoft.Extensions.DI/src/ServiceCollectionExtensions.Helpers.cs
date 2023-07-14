using DotEnv.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    private static IEnvironmentVariablesProvider Load(params string[] paths)
        => new EnvLoader().AddEnvFiles(paths).Load();

    private static IEnvironmentVariablesProvider LoadEnv(string basePath = null, string environmentName = null)
    {
        var loader = new EnvLoader();
        if (basePath is not null)
            loader.SetBasePath(basePath);
        if (environmentName is not null)
            loader.SetEnvironmentName(environmentName);
        return loader.LoadEnv();
    }

    /// <summary>
    /// This registers <see cref="IEnvReader" /> as a singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="provider">The environment variables provider.</param>
    /// <returns>An instance that allows access to the environment variables.</returns>
    private static IEnvReader AddEnvReader(this IServiceCollection services, IEnvironmentVariablesProvider provider)
    {
        var reader = new EnvReader(provider);
        services.AddSingleton<IEnvReader>(reader);
        return reader;
    }

    /// <summary>
    /// This registers <typeparamref name="TSettings" /> as a singleton.
    /// </summary>
    /// <typeparam name="TSettings">The type of the new instance to bind.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="provider">The environment variables provider.</param>
    /// <returns>An instance that allows access to the environment variables.</returns>
    private static TSettings AddTSettings<TSettings>(this IServiceCollection services, IEnvironmentVariablesProvider provider)
        where TSettings : class, new()
    {
        var settings = new EnvBinder(provider).Bind<TSettings>();
        services.AddSingleton(settings);
        return settings;
    }
}