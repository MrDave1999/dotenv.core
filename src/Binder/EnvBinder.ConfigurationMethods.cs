namespace DotEnv.Core;

// This class defines the configuration methods that will be used to change the behavior of the binder.
public partial class EnvBinder
{
    /// <inheritdoc />
    public IEnvBinder IgnoreException()
    {
        _configuration.ThrowException = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvBinder AllowBindNonPublicProperties()
    {
        _configuration.BindNonPublicProperties = true;
        return this;
    }
}
