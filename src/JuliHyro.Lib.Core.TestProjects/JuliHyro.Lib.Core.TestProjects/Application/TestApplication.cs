
using System;
using System.Diagnostics;
using System.Globalization;
using JuliHyro.Lib.Core.TestProjects.Core;
using JuliHyro.Lib.Core.TestProjects.Models;
using JuliHyro.Lib.Core.TestProjects.Registry;
using JuliHyro.Lib.Core.TestProjects.Tests;

namespace JuliHyro.Lib.Core.TestProjects.Application
{
    public sealed class TestApplication
    {
        private readonly TestRegistry _registry = new();

        public void Run()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            RegisterTestModes();
            ShowPrompt();
        }

        private void RegisterTestModes()
        {
            ConverterTestSuite.Register(_registry);
        }

        private void ShowPrompt()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("==================================================");
                Console.WriteLine("       JuliHyro.Lib.Core Testing Framework");
                Console.WriteLine("==================================================");
                Console.WriteLine();
                Console.WriteLine("Available Test Modes:");
                Console.WriteLine();

                var modes = _registry.GetAll();

                for (int i = 0; i < modes.Count; i++)
                {
                    Console.WriteLine($"  [{i + 1}] {modes[i].Name}");
                    Console.WriteLine($"      {modes[i].Description}");
                    Console.WriteLine();
                }

                Console.WriteLine("  [0] Exit");
                Console.WriteLine();
                Console.Write("Select test mode: ");

                string? input = Console.ReadLine();

                if (input == "0")
                    return;

                if (!int.TryParse(input, out int selected) ||
                    selected < 1 ||
                    selected > modes.Count)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid selection.");
                    Pause();
                    continue;
                }

                ExecuteMode(modes[selected - 1]);
            }
        }

        private static void ExecuteMode(TestMode mode)
        {
            Console.Clear();

            var session = new TestSession(mode);

            Console.WriteLine("==================================================");
            Console.WriteLine(" TEST SESSION");
            Console.WriteLine("==================================================");
            Console.WriteLine($"Mode        : {mode.Name}");
            Console.WriteLine($"Mode ID     : {mode.Id}");
            Console.WriteLine($"Started     : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine("==================================================");

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                mode.Execute(session);
            }
            catch (System.Exception ex)
            {
                session.RecordInfrastructureFailure(
                    "Unhandled test mode exception",
                    ex
                );
            }
            finally
            {
                stopwatch.Stop();
                session.PrintSummary(stopwatch.Elapsed);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return to the test prompt...");
            Console.ReadKey(intercept: true);
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}
