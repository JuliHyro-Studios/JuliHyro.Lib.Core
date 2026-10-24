using System;

namespace JuliHyro.Lib.Core.Exception
{
    public class EnumSameException : System.Exception
    {
        public EnumSameException(string message)
            : base(message)
        {
        }

        public EnumSameException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}