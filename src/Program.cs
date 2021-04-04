namespace Identity.Business
{
    using AlbedoTeam.Sdk.JobWorker;

    internal static class Program
    {
        private static void Main()
        {
            Worker.Configure<Startup>().Run();
        }
    }
}