using Microsoft.Extensions.Configuration;

namespace SmartoLifeCRM.App.Services;

public sealed class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ApplicationName => _configuration["Application:Name"] ?? "Smarto Life CRM";

    public string Version => _configuration["Application:Version"] ?? "v0.3.0";

    public string EnvironmentName => _configuration["Application:Environment"] ?? "Local Desktop";
}

