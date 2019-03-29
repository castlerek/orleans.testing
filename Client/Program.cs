namespace App.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class Program
    {
        private const string CLUSTER_ID = "dev";
        private const string SERVICE_ID = "TestService";
        private const int DEFAULT_PRIMARY_KEY = 0;
        private const int DEFAULT_DELAY = 5;
        private static readonly AutoResetEvent mre = new AutoResetEvent(false);

        [MTAThread]
        public static void Main()
        {
            try
            {
                Task.Factory.StartNew(async() =>
                {
                    var client = new ChemicalClient(CLUSTER_ID, SERVICE_ID);
                    await client.Initialize();
                    client.HandleWork();
                });

                mre.WaitOne();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
