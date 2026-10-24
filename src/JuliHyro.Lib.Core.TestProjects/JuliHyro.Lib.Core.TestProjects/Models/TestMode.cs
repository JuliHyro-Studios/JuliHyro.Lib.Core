
using System;
using JuliHyro.Lib.Core.TestProjects.Core;

namespace JuliHyro.Lib.Core.TestProjects.Models
{
    public sealed class TestMode
    {
        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public Action<TestSession> Execute { get; }

        public TestMode(
            string id,
            string name,
            string description,
            Action<TestSession> execute)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mode ID cannot be empty.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Mode name cannot be empty.", nameof(name));

            Id = id;
            Name = name;
            Description = description;
            Execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }
    }
}
