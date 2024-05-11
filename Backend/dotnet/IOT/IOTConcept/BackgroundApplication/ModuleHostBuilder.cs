using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackgroundApplication
{
    public class ModuleHostBuilder : IHostBuilder
    {
        private readonly IHostBuilder _hostBuilder;

        public ModuleHostBuilder(string[] args)
        {
            _hostBuilder = Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime();
        }
        public IDictionary<object, object> Properties => throw new NotImplementedException();
        public IHost Build() => _hostBuilder.Build();
        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate) =>
            _hostBuilder.ConfigureAppConfiguration(configureDelegate);


        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate) =>
            _hostBuilder.ConfigureContainer(configureDelegate);


        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate) =>
            _hostBuilder.ConfigureHostConfiguration(configureDelegate);

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate) =>
            _hostBuilder.ConfigureServices(configureDelegate);

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
            where TContainerBuilder : notnull => _hostBuilder.UseServiceProviderFactory(factory);

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
            where TContainerBuilder : notnull => _hostBuilder.UseServiceProviderFactory(factory);
    }
}
