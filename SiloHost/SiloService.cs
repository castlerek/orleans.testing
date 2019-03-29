namespace App.SiloHost
{
    using System.Threading.Tasks;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class SiloService
    {
        private readonly string _clusterId;
        private readonly string _serviceId;
        private readonly ISiloHostBuilder _builder;

        public SiloService(string clusterId, string serviceId)
        {
            this._clusterId = clusterId;
            this._serviceId = serviceId;
            this._builder = Initialize();
        }

        public async Task<ISiloHost> CreateHost()
        {
            var host = _builder.Build();
            await host.StartAsync();
            return host;
        }

        private ISiloHostBuilder Initialize()
        {
            var assembly = typeof(App.Grains.FormulaRecognizerGrain).Assembly;

            return new SiloHostBuilder()
            .UseLocalhostClustering()
            .Configure<ClusterOptions>(options => { options.ClusterId = this._clusterId; options.ServiceId = this._serviceId; })
            .ConfigureApplicationParts(parts => parts.AddApplicationPart(assembly).WithReferences())
            .ConfigureLogging(logging => logging.AddConsole());
        }
    }
}
