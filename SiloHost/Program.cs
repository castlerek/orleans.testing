namespace App.SiloHost
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class Program
    {
        private const string CLUSTER_ID = "dev";
        private const string SERVICE_ID = "TestService";

        private static ManualResetEvent mre = new ManualResetEvent(false);

        [MTAThread]
        public static void Main()
        {
            Task.Factory.StartNew(async() =>
            {
                var service = new SiloService(CLUSTER_ID, SERVICE_ID);
                await service.CreateHost();
            });

            mre.WaitOne();
        }
    }
}
