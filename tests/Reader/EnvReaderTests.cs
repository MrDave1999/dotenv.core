namespace DotEnv.Core.Tests.Reader;

[TestClass]
public partial class EnvReaderTests
{
    private const string VariableNotFound = nameof(VariableNotFound);

    [TestMethod]
    public void HasValue_WhenVariableExistsInTheCurrentProcess_ShouldReturnTrue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("VARIABLE_NAME", "1");
        Assert.AreEqual(expected: true, actual: reader.HasValue("VARIABLE_NAME"));
        SetEnvironmentVariable("VARIABLE_NAME", null);
        Assert.AreEqual(expected: false, actual: reader.HasValue("VARIABLE_NAME"));
        Assert.AreEqual(expected: false, actual: reader.HasValue(""));
    }
}
