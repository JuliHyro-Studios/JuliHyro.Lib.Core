using System;
namespace JuliHyro.Lib.Core.Exception
{
    public class CannotException : System.Exception
    {
        public CannotException(string message)
            : base(message)
        {
        }

        public CannotException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}