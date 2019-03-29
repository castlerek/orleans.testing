namespace App.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Orleans;
    using Orleans.Configuration;
    using Orleans.Runtime;
    using Microsoft.Extensions.Logging;

    public sealed class ChemicalClient
    {
        private const int DEFAULT_PRIMARY_KEY = 0;
        private const int DEFAULT_DELAY = 5;

        private readonly string _clusterId;
        private readonly string _serviceId;

        private IClusterClient _clusterClient;

        public ChemicalClient(string clusterId, string serviceId)
        {
            this._clusterId = clusterId;
            this._serviceId = serviceId;
        }

        public async Task<IClusterClient> Initialize(int initializeAttemptsBeforeFailing = 5)
        {
            IClusterClient client;
            int attempt = 0;

            while (true)
            {
                try
                {
                    client = new ClientBuilder()
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = this._clusterId;
                        options.ServiceId = this._serviceId;
                    })
                    .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(App.Interfaces.IFormulaRecognizer).Assembly).WithReferences())
                    .ConfigureLogging(logging => logging.AddConsole())
                    .Build();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to the Silo-host.");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");

                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(DEFAULT_DELAY));
                }
            }

            this._clusterClient = client;
            return this._clusterClient;
        }

        public List<string> HandleWork()
        {
            if (this._clusterClient == null)
                throw new Exception("Cluster client isn't initialized, can't handle work.");

            var grain     = this._clusterClient.GetGrain<App.Interfaces.IFormulaRecognizer>(DEFAULT_PRIMARY_KEY);
            var formulas  = new List<string>() { "H2O2", "CH3", "C3H6" };
            var responses = new List<string>();

            formulas.ForEach(async item => {
                var name = await grain.GetChemicalSubstanceName(item);
                responses.Add(name);
                Console.WriteLine(name);
            });

            return responses;
        }
    }
}
