namespace DotEnv.Core.Tests.Reader;

[TestClass]
public class EnvAccessorExtensionsTests
{
    private void GetEnvTestHelper(string expected)
    {
        // Arrange
        var variable = "GET_ENV_TEST_HELPER";
        SetEnvironmentVariable(variable, expected);

        // Act
        var actual = variable.GetEnv();

        // Assert
        actual.Should().Be(expected);
    }

    private void GetEnvTestHelper<T>(string value, T expected)
        where T : IConvertible
    {
        // Arrange
        var variable = "GET_ENV_TEST_HELPER";
        SetEnvironmentVariable(variable, value);

        // Act
        var actual = variable.GetEnv<T>();

        // Assert
        actual.Should().Be(expected);
    }

    [TestMethod]
    public void GetEnv_WhenVariableExistsInCurrentProcess_ShouldReturnsValue()
    {
        GetEnvTestHelper(expected: "Test");
        GetEnvTestHelper<string>(value:  "Test",  expected: "Test");
        GetEnvTestHelper<bool>(value:    "true",  expected: true);
        GetEnvTestHelper<bool>(value:    "false", expected: false);
        GetEnvTestHelper<byte>(value:    "10",    expected: 10);
        GetEnvTestHelper<sbyte>(value:   "-10",   expected: -10);
        GetEnvTestHelper<char>(value:    "A",     expected: 'A');
        GetEnvTestHelper<ushort>(value:  "20",    expected: 20);
        GetEnvTestHelper<short>(value:   "-20",   expected: -20);
        GetEnvTestHelper<uint>(value:    "200",   expected: 200);
        GetEnvTestHelper<int>(value:     "-200",  expected: -200);
        GetEnvTestHelper<ulong>(value:   "5000",  expected: 5000UL);
        GetEnvTestHelper<long>(value:    "-5000", expected: -5000L);
        GetEnvTestHelper<float>(value:   "12.5",  expected: 12.5F);
        GetEnvTestHelper<double>(value:  "12.5",  expected: 12.5D);
        GetEnvTestHelper<decimal>(value: "12.5",  expected: 12.5M);
        GetEnvTestHelper<Colors>(value:  "RED",   expected: Colors.Red);
        GetEnvTestHelper<Colors>(value:  "BLUE",  expected: Colors.Blue);
    }

    private enum Colors
    {
        Red,
        Blue
    }
}
