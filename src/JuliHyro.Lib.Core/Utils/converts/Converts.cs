using System;
using JuliHyro.Lib.Core.Exception;
namespace JuliHyro.Lib.Core.Utils.converts
{
    public static class Converts
    {
        public enum InputConvertType
        {
            String,
            Int,
            Double,
            Float,
            Decimal,
            Bool,
            DateTime 
        }
        public enum OutputConvertType
        {
            String,
            Int,
            Double,
            Float,
            Decimal,
            Bool,
            DateTime
        }
        /// <summary>
        /// Converts the input value to the specified output type.
        /// </summary>
        /// <param name="inputType">The type of the input value.</param>
        /// <param name="outputType">The desired output type.</param>
        /// <param name="inputValue">The value to be converted.</param>
        public static string Convert(InputConvertType inputType, OutputConvertType outputType, string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                throw new ArgumentException("Input value cannot be null or empty.", nameof(inputValue));
            }   
            if (!Enum.IsDefined(typeof(InputConvertType), inputType))
            {
                throw new ArgumentOutOfRangeException(nameof(inputType), "Invalid input type.");
            }
            if (!Enum.IsDefined(typeof(OutputConvertType), outputType))
            {
                throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
            if ((int)inputType == (int)outputType)
            {
                throw new EnumSameException($"Input type '{inputType}' and output type '{outputType}' are the same.");
            }
            switch (inputType)
            {
                case InputConvertType.String:
                    return ConvertFromString(outputType, inputValue);
                case InputConvertType.Int:
                    return ConvertFromInt(outputType, inputValue);
                case InputConvertType.Double:
                    return ConvertFromDouble(outputType, inputValue);
                case InputConvertType.Float:
                    return ConvertFromFloat(outputType, inputValue);
                case InputConvertType.Decimal:
                    return ConvertFromDecimal(outputType, inputValue);
                case InputConvertType.Bool:
                    return ConvertFromBool(outputType, inputValue);
                case InputConvertType.DateTime:
                    return ConvertFromDateTime(outputType, inputValue);
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), "Invalid input type.");
            }
        }
        private static string ConvertFromString(OutputConvertType outputType, string inputValue)
        {
            switch (outputType)
            {
                case OutputConvertType.Int:
                    if (int.TryParse(inputValue, out int intValue))
                    {
                        return intValue.ToString();
                    }
                    throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Int.");
                case OutputConvertType.Double:
                    if (double.TryParse(inputValue, out double doubleValue))
                    {
                        return doubleValue.ToString();
                    }
                    throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Double.");
                case OutputConvertType.Float:
                    if (float.TryParse(inputValue, out float floatValue))
                    {
                        return floatValue.ToString();
                    }
                    throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Float.");
                case OutputConvertType.Decimal:
                    if (decimal.TryParse(inputValue, out decimal decimalValue))
                    {
                        return decimalValue.ToString();
                    }
                    throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Decimal.");
                case OutputConvertType.Bool:
                    if (bool.TryParse(inputValue, out bool boolValue))
                    {
                        return boolValue.ToString();
                    }
                    throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Bool.");
                case OutputConvertType.DateTime:
                    if (DateTime.TryParse(inputValue, out DateTime dateTimeValue))
                    {
                        return dateTimeValue.ToString("o"); // ISO 8601 format
                    }
                    throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to DateTime.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
        private static string ConvertFromInt(OutputConvertType outputType, string inputValue)
        {
            if (!int.TryParse(inputValue, out int intValue))
            {
                throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Int.");
            }
            switch (outputType)
            {
                case OutputConvertType.String:
                    return intValue.ToString();
                case OutputConvertType.Double:
                    return ((double)intValue).ToString();
                case OutputConvertType.Float:
                    return ((float)intValue).ToString();
                case OutputConvertType.Decimal:
                    return ((decimal)intValue).ToString();
                case OutputConvertType.Bool:
                    return (intValue != 0).ToString();
                case OutputConvertType.DateTime:
                    throw new InvalidOperationException("Cannot convert Int to DateTime.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
        private static string ConvertFromDouble(OutputConvertType outputType, string inputValue)
        {
            if (!double.TryParse(inputValue, out double doubleValue))
            {
                throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Double.");
            }
            switch (outputType)
            {
                case OutputConvertType.String:
                    return doubleValue.ToString();
                case OutputConvertType.Int:
                    return ((int)doubleValue).ToString();
                case OutputConvertType.Float:
                    return ((float)doubleValue).ToString();
                case OutputConvertType.Decimal:
                    return ((decimal)doubleValue).ToString();
                case OutputConvertType.Bool:
                    return (doubleValue != 0).ToString();
                case OutputConvertType.DateTime:
                    throw new InvalidOperationException("Cannot convert Double to DateTime.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
        private static string ConvertFromFloat(OutputConvertType outputType, string inputValue)
        {
            if (!float.TryParse(inputValue, out float floatValue))
            {
                throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Float.");
            }
            switch (outputType)
            {
                case OutputConvertType.String:
                    return floatValue.ToString();
                case OutputConvertType.Int:
                    return ((int)floatValue).ToString();
                case OutputConvertType.Double:
                    return ((double)floatValue).ToString();
                case OutputConvertType.Decimal:
                    return ((decimal)floatValue).ToString();
                case OutputConvertType.Bool:
                    return (floatValue != 0).ToString();
                case OutputConvertType.DateTime:
                    throw new InvalidOperationException("Cannot convert Float to DateTime.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
        private static string ConvertFromDecimal(OutputConvertType outputType, string inputValue)
        {
            if (!decimal.TryParse(inputValue, out decimal decimalValue))
            {
                throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Decimal.");
            }
            switch (outputType)
            {
                case OutputConvertType.String:
                    return decimalValue.ToString();
                case OutputConvertType.Int:
                    return ((int)decimalValue).ToString();
                case OutputConvertType.Double:
                    return ((double)decimalValue).ToString();
                case OutputConvertType.Float:
                    return ((float)decimalValue).ToString();
                case OutputConvertType.Bool:
                    return (decimalValue != 0).ToString();
                case OutputConvertType.DateTime:
                    throw new InvalidOperationException("Cannot convert Decimal to DateTime.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
        private static string ConvertFromBool(OutputConvertType outputType, string inputValue)
        {
            if (!bool.TryParse(inputValue, out bool boolValue))
            {
                throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to Bool.");
            }
            switch (outputType)
            {
                case OutputConvertType.String:
                    return boolValue.ToString();
                case OutputConvertType.Int:
                    return (boolValue ? 1 : 0).ToString();
                case OutputConvertType.Double:
                    return (boolValue ? 1.0 : 0.0).ToString();
                case OutputConvertType.Float:
                    return (boolValue ? 1.0f : 0.0f).ToString();
                case OutputConvertType.Decimal:
                    return (boolValue ? 1m : 0m).ToString();
                case OutputConvertType.DateTime:
                    throw new InvalidOperationException("Cannot convert Bool to DateTime.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
        private static string ConvertFromDateTime(OutputConvertType outputType, string inputValue)
        {
            if (!DateTime.TryParse(inputValue, out DateTime dateTimeValue))
            {
                throw new JuliHyro.Lib.Core.Exception.CannotException($"Cannot convert '{inputValue}' to DateTime.");
            }
            switch (outputType)
            {
                case OutputConvertType.String:
                    return dateTimeValue.ToString("o"); // ISO 8601 format
                case OutputConvertType.Int:
                    throw new InvalidOperationException("Cannot convert DateTime to Int.");
                case OutputConvertType.Double:
                    throw new InvalidOperationException("Cannot convert DateTime to Double.");
                case OutputConvertType.Float:
                    throw new InvalidOperationException("Cannot convert DateTime to Float.");
                case OutputConvertType.Decimal:
                    throw new InvalidOperationException("Cannot convert DateTime to Decimal.");
                case OutputConvertType.Bool:
                    throw new InvalidOperationException("Cannot convert DateTime to Bool.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputType), "Invalid output type.");
            }
        }
    }
}