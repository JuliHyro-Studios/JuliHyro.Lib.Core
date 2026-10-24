
using System;
using System.Collections.Generic;
using System.Linq;
using JuliHyro.Lib.Core.TestProjects.Models;

namespace JuliHyro.Lib.Core.TestProjects.Registry
{
    public sealed class TestRegistry
    {
        private readonly List<TestMode> _modes = new();

        public int Count => _modes.Count;

        public void Register(TestMode mode)
        {
            ArgumentNullException.ThrowIfNull(mode);

            bool exists = _modes.Any(m =>
                string.Equals(
                    m.Id,
                    mode.Id,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Test mode '{mode.Id}' is already registered."
                );
            }

            _modes.Add(mode);
        }

        public IReadOnlyList<TestMode> GetAll()
        {
            return _modes.AsReadOnly();
        }

        public TestMode? FindById(string id)
        {
            return _modes.FirstOrDefault(m =>
                string.Equals(
                    m.Id,
                    id,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }
    }
}
