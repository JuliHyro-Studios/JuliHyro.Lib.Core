
using JuliHyro.Lib.Core.TestProjects.Application;

namespace JuliHyro.Lib.Core.TestProjects
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var application = new TestApplication();
            application.Run();
        }
    }
}
