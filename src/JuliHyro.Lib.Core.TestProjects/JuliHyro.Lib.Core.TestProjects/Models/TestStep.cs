
using System;
using System.Collections.Generic;

namespace JuliHyro.Lib.Core.TestProjects.Models
{
    public sealed class TestStep
    {
        public int Number { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Type { get; init; } = string.Empty;

        public StepStatus Status { get; set; }

        public string? ExpectedException { get; init; }

        public string? ActualException { get; set; }

        public string? ExceptionMessage { get; set; }

        public List<string> Messages { get; } = new();

        public TimeSpan Duration { get; set; }
    }
}
