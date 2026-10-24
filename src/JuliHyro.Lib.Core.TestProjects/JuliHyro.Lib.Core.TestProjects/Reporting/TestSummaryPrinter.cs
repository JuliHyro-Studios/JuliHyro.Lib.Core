
using System;
using JuliHyro.Lib.Core.TestProjects.Core;
using JuliHyro.Lib.Core.TestProjects.Models;

namespace JuliHyro.Lib.Core.TestProjects.Reporting
{
    public sealed class TestSummaryPrinter
    {
        public void Print(TestSession session, TimeSpan totalDuration)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("======================================================================");
            Console.WriteLine($" TEST REPORT: {session.Mode.Name}");
            Console.WriteLine("======================================================================");

            PrintStepTable(session);
            PrintStepMessages(session);

            PrintSummary(session, totalDuration);

            Console.WriteLine();
            Console.WriteLine("======================================================================");

            if (session.FailedSteps == 0)
                Console.WriteLine(" RESULT: ALL TESTS PASSED");
            else
                Console.WriteLine($" RESULT: {session.FailedSteps} STEP(S) FAILED");

            Console.WriteLine("======================================================================");
        }

        private static void PrintStepTable(TestSession session)
        {
            Console.WriteLine();
            Console.WriteLine("STEP SUMMARY");
            Console.WriteLine(
                "-----------------------------------------------------------------------"
            );

            Console.WriteLine(
                $"{"NO.",-5} {"TYPE",-20} {"STATUS",-8} {"DURATION",-14} NAME"
            );

            Console.WriteLine(
                "-----------------------------------------------------------------------"
            );

            foreach (TestStep step in session.Steps)
            {
                Console.WriteLine(
                    $"{step.Number,-5} " +
                    $"{Truncate(step.Type, 20),-20} " +
                    $"{step.Status,-8} " +
                    $"{step.Duration.TotalMilliseconds,9:F2} ms   " +
                    $"{step.Name}"
                );
            }

            Console.WriteLine(
                "-----------------------------------------------------------------------"
            );
        }

        private static void PrintStepMessages(TestSession session)
        {
            Console.WriteLine();
            Console.WriteLine("STEP MESSAGE DETAILS");
            Console.WriteLine(
                "-----------------------------------------------------------------------"
            );

            foreach (TestStep step in session.Steps)
            {
                Console.WriteLine();
                Console.WriteLine($"STEP {step.Number:D3}: {step.Name}");
                Console.WriteLine($"Type   : {step.Type}");
                Console.WriteLine($"Status : {step.Status}");

                if (step.ExpectedException != null)
                    Console.WriteLine($"Expected Exception: {step.ExpectedException}");

                if (step.ActualException != null)
                    Console.WriteLine($"Actual Exception  : {step.ActualException}");

                if (step.ExceptionMessage != null)
                    Console.WriteLine($"Exception Message : {step.ExceptionMessage}");

                if (step.Messages.Count == 0)
                {
                    Console.WriteLine("Messages: None");
                    continue;
                }

                Console.WriteLine("Messages:");

                foreach (string message in step.Messages)
                    Console.WriteLine($"  - {message}");
            }

            Console.WriteLine();
            Console.WriteLine(
                "-----------------------------------------------------------------------"
            );
            Console.WriteLine("END OF STEP DETAILS");
            Console.WriteLine(
                "-----------------------------------------------------------------------"
            );
        }

        private static void PrintSummary(
            TestSession session,
            TimeSpan totalDuration)
        {
            double passRate = session.TotalSteps == 0
                ? 0
                : (double)session.PassedSteps / session.TotalSteps * 100;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("======================================================================");
            Console.WriteLine($" SUMMARY: {session.Mode.Name}");
            Console.WriteLine("======================================================================");

            Console.WriteLine($"Mode ID          : {session.Mode.Id}");
            Console.WriteLine($"Total Steps      : {session.TotalSteps}");
            Console.WriteLine($"Passed           : {session.PassedSteps}");
            Console.WriteLine($"Failed           : {session.FailedSteps}");
            Console.WriteLine($"Pass Rate        : {passRate:F2}%");
            Console.WriteLine($"Execution Time   : {totalDuration.TotalMilliseconds:F2} ms");

            Console.WriteLine("======================================================================");
        }

        private static string Truncate(string value, int maxLength)
        {
            if (value.Length <= maxLength)
                return value;

            return value[..(maxLength - 3)] + "...";
        }
    }
}
