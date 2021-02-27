using AlbedoTeam.Sdk.JobWorker;

namespace Identity.Business
{
    internal static class Program
    {
        private static void Main()
        {
            Worker.Configure<Startup>().Run();
        }
    }
}