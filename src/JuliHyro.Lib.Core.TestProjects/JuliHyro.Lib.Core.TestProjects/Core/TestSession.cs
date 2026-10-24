
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JuliHyro.Lib.Core.TestProjects.Models;
using JuliHyro.Lib.Core.TestProjects.Reporting;

namespace JuliHyro.Lib.Core.TestProjects.Core
{
    public sealed class TestSession
    {
        private readonly List<TestStep> _steps = new();
        private readonly TestSummaryPrinter _summaryPrinter = new();
        private TestStep? _currentStep;

        public TestMode Mode { get; }

        public int TotalSteps => _steps.Count;

        public int PassedSteps =>
            _steps.Count(s => s.Status == StepStatus.Passed);

        public int FailedSteps =>
            _steps.Count(s => s.Status == StepStatus.Failed);

        public IReadOnlyList<TestStep> Steps => _steps.AsReadOnly();

        public TestSession(TestMode mode)
        {
            Mode = mode ?? throw new ArgumentNullException(nameof(mode));
        }

        public void PrintGroup(string groupName)
        {
            Console.WriteLine();
            Console.WriteLine($"[TEST GROUP] {groupName}");
            Console.WriteLine(new string('-', 60));
        }

        public void Log(string message)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (_currentStep == null)
            {
                Console.WriteLine(message);
                return;
            }

            _currentStep.Messages.Add(message);
            Console.WriteLine($"         {message}");
        }

        public void RunStep(
            string name,
            Action action,
            string? expectedException = null,
            string? stepType = null)
        {
            ArgumentNullException.ThrowIfNull(action);

            var step = new TestStep
            {
                Number = _steps.Count + 1,
                Name = name,
                Type = stepType ?? Mode.Name,
                Status = StepStatus.Failed,
                ExpectedException = expectedException
            };

            _steps.Add(step);
            _currentStep = step;

            Stopwatch stopwatch = Stopwatch.StartNew();

            Console.WriteLine();
            Console.WriteLine(
                $"[STEP {step.Number:D3}] [{step.Type}] {step.Name}"
            );

            try
            {
                action();

                if (expectedException != null)
                {
                    step.Status = StepStatus.Failed;

                    Log(
                        $"Expected exception '{expectedException}', " +
                        "but no exception was thrown."
                    );
                }
                else
                {
                    step.Status = StepStatus.Passed;
                }
            }
            catch (System.Exception ex)
            {
                step.ActualException = ex.GetType().Name;
                step.ExceptionMessage = ex.Message;

                bool matchesExpected =
                    expectedException != null &&
                    string.Equals(
                        expectedException,
                        ex.GetType().Name,
                        StringComparison.Ordinal
                    );

                if (matchesExpected)
                {
                    step.Status = StepStatus.Passed;
                    Log($"Expected exception: {ex.GetType().Name}");
                    Log($"Exception message: {ex.Message}");
                }
                else
                {
                    step.Status = StepStatus.Failed;
                    Log($"Exception: {ex.GetType().Name}");
                    Log($"Exception message: {ex.Message}");

                    if (expectedException != null)
                        Log($"Expected exception: {expectedException}");
                }
            }
            finally
            {
                stopwatch.Stop();
                step.Duration = stopwatch.Elapsed;
                _currentStep = null;

                Console.WriteLine(
                    $"         STATUS: [{step.Status}] " +
                    $"| {step.Duration.TotalMilliseconds:F2} ms"
                );
            }
        }

        public void RecordInfrastructureFailure(
            string name,
            System.Exception exception)
        {
            var step = new TestStep
            {
                Number = _steps.Count + 1,
                Name = name,
                Type = Mode.Name,
                Status = StepStatus.Failed,
                ActualException = exception.GetType().Name,
                ExceptionMessage = exception.Message
            };

            step.Messages.Add(exception.ToString());
            _steps.Add(step);

            Console.WriteLine();
            Console.WriteLine($"[STEP {step.Number:D3}] [FAILED] {name}");
            Console.WriteLine(exception);
        }

        public void PrintSummary(TimeSpan totalDuration)
        {
            _summaryPrinter.Print(this, totalDuration);
        }
    }
}
