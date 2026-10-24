
using System;
using JuliHyro.Lib.Core.TestProjects.Core;
using JuliHyro.Lib.Core.TestProjects.Models;
using JuliHyro.Lib.Core.TestProjects.Registry;
using JuliHyro.Lib.Core.Utils.converts;

namespace JuliHyro.Lib.Core.TestProjects.Tests
{
    public static class ConverterTestSuite
    {
        public static void Register(TestRegistry registry)
        {
            registry.Register(
                new TestMode(
                    id: "converter",
                    name: "Converter Test",
                    description: "Test all Converter input/output combinations.",
                    execute: Run
                )
            );
        }

        private static void Run(TestSession session)
        {
            TestAllConverterCombinations(session);
            TestInvalidInputValues(session);
            TestInvalidEnumValues(session);
            TestUnsupportedConversions(session);
        }

        private static void TestAllConverterCombinations(TestSession session)
        {
            session.PrintGroup("All Converter Combinations");

            Converts.InputConvertType[] inputTypes =
            {
                Converts.InputConvertType.String,
                Converts.InputConvertType.Int,
                Converts.InputConvertType.Double,
                Converts.InputConvertType.Float,
                Converts.InputConvertType.Decimal,
                Converts.InputConvertType.Bool,
                Converts.InputConvertType.DateTime
            };

            Converts.OutputConvertType[] outputTypes =
            {
                Converts.OutputConvertType.String,
                Converts.OutputConvertType.Int,
                Converts.OutputConvertType.Double,
                Converts.OutputConvertType.Float,
                Converts.OutputConvertType.Decimal,
                Converts.OutputConvertType.Bool,
                Converts.OutputConvertType.DateTime
            };

            foreach (var inputType in inputTypes)
            {
                foreach (var outputType in outputTypes)
                {
                    string inputValue = GetSampleValue(inputType, outputType);
                    bool supported = IsConversionSupported(inputType, outputType);

                    string? expectedException = supported
                        ? null
                        : (int)inputType == (int)outputType
                            ? "EnumSameException"
                            : "InvalidOperationException";

                    session.RunStep(
                        $"{inputType} -> {outputType}",
                        () =>
                        {
                            string result = Converts.Convert(
                                inputType,
                                outputType,
                                inputValue
                            );

                            session.Log($"Input : {inputValue}");
                            session.Log($"Output: {result}");

                            if (string.IsNullOrEmpty(result))
                            {
                                throw new InvalidOperationException(
                                    "Converter returned a null or empty result."
                                );
                            }
                        },
                        expectedException
                    );
                }
            }
        }

        private static string GetSampleValue(
            Converts.InputConvertType inputType,
            Converts.OutputConvertType outputType)
        {
            if (inputType == Converts.InputConvertType.String)
            {
                return outputType switch
                {
                    Converts.OutputConvertType.String => "Hello World",
                    Converts.OutputConvertType.Int => "123",
                    Converts.OutputConvertType.Double => "123.5",
                    Converts.OutputConvertType.Float => "123.5",
                    Converts.OutputConvertType.Decimal => "123.5",
                    Converts.OutputConvertType.Bool => "true",
                    Converts.OutputConvertType.DateTime =>
                        "2026-10-24T12:30:00.0000000",
                    _ => "123"
                };
            }

            return inputType switch
            {
                Converts.InputConvertType.Int => "123",
                Converts.InputConvertType.Double => "123.5",
                Converts.InputConvertType.Float => "123.5",
                Converts.InputConvertType.Decimal => "123.5",
                Converts.InputConvertType.Bool => "true",
                Converts.InputConvertType.DateTime =>
                    "2026-10-24T12:30:00.0000000",
                _ => "123"
            };
        }

        private static bool IsConversionSupported(
            Converts.InputConvertType inputType,
            Converts.OutputConvertType outputType)
        {
            if ((int)inputType == (int)outputType)
                return false;

            if (inputType == Converts.InputConvertType.String)
                return true;

            if (inputType == Converts.InputConvertType.DateTime)
            {
                return outputType == Converts.OutputConvertType.String;
            }

            if (outputType == Converts.OutputConvertType.DateTime)
                return false;

            return true;
        }

        private static void TestInvalidInputValues(TestSession session)
        {
            session.PrintGroup("Invalid Input Values");

            session.RunStep(
                "Empty string input",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Int,
                    ""
                ),
                "ArgumentException"
            );

            session.RunStep(
                "Null string input",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Int,
                    null!
                ),
                "ArgumentException"
            );

            session.RunStep(
                "Invalid integer string",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Int,
                    "not-an-integer"
                ),
                "CannotException"
            );

            session.RunStep(
                "Invalid double string",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Double,
                    "not-a-double"
                ),
                "CannotException"
            );

            session.RunStep(
                "Invalid float string",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Float,
                    "not-a-float"
                ),
                "CannotException"
            );

            session.RunStep(
                "Invalid decimal string",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Decimal,
                    "not-a-decimal"
                ),
                "CannotException"
            );

            session.RunStep(
                "Invalid boolean string",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.Bool,
                    "maybe"
                ),
                "CannotException"
            );

            session.RunStep(
                "Invalid DateTime string",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    Converts.OutputConvertType.DateTime,
                    "not-a-date"
                ),
                "CannotException"
            );
        }

        private static void TestInvalidEnumValues(TestSession session)
        {
            session.PrintGroup("Invalid Enum Values");

            session.RunStep(
                "Invalid InputConvertType",
                () => Converts.Convert(
                    (Converts.InputConvertType)999,
                    Converts.OutputConvertType.String,
                    "123"
                ),
                "ArgumentOutOfRangeException"
            );

            session.RunStep(
                "Invalid OutputConvertType",
                () => Converts.Convert(
                    Converts.InputConvertType.String,
                    (Converts.OutputConvertType)999,
                    "123"
                ),
                "ArgumentOutOfRangeException"
            );
        }

        private static void TestUnsupportedConversions(TestSession session)
        {
            session.PrintGroup("Unsupported Conversions");

            var cases = new (
                Converts.InputConvertType Input,
                Converts.OutputConvertType Output,
                string Value
            )[]
            {
                (Converts.InputConvertType.Int,
                 Converts.OutputConvertType.DateTime, "123"),

                (Converts.InputConvertType.Double,
                 Converts.OutputConvertType.DateTime, "123.5"),

                (Converts.InputConvertType.Float,
                 Converts.OutputConvertType.DateTime, "123.5"),

                (Converts.InputConvertType.Decimal,
                 Converts.OutputConvertType.DateTime, "123.5"),

                (Converts.InputConvertType.Bool,
                 Converts.OutputConvertType.DateTime, "true"),

                (Converts.InputConvertType.DateTime,
                 Converts.OutputConvertType.Int, "2026-10-24T12:30:00.0000000"),

                (Converts.InputConvertType.DateTime,
                 Converts.OutputConvertType.Double, "2026-10-24T12:30:00.0000000"),

                (Converts.InputConvertType.DateTime,
                 Converts.OutputConvertType.Float, "2026-10-24T12:30:00.0000000"),

                (Converts.InputConvertType.DateTime,
                 Converts.OutputConvertType.Decimal, "2026-10-24T12:30:00.0000000"),

                (Converts.InputConvertType.DateTime,
                 Converts.OutputConvertType.Bool, "2026-10-24T12:30:00.0000000")
            };

            foreach (var test in cases)
            {
                session.RunStep(
                    $"{test.Input} -> {test.Output}",
                    () => Converts.Convert(
                        test.Input,
                        test.Output,
                        test.Value
                    ),
                    "InvalidOperationException"
                );
            }
        }
    }
}
