using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace DotEnv.Core.Tests.Parser
{
    [TestClass]
    public class EnvParserOptionsTests
    {
        [TestMethod]
        public void Parse_WhenDisabledTrimStartValues_ShouldNotIgnoreLeadingWhitepaces()
        {
            string env = "TRIM_START_VALUES=   1";

            new EnvParser()
                .DisableTrimStartValues()
                .Parse(env);

            Assert.AreEqual("   1", GetEnvironmentVariable("TRIM_START_VALUES"));
        }

        [TestMethod]
        public void Parse_WhenDisabledTrimEndValues_ShouldNotIgnoreTrailingWhitepaces()
        {
            string env = "TRIM_END_VALUES=1   ";

            new EnvParser()
                .DisableTrimEndValues()
                .Parse(env);

            Assert.AreEqual("1   ", GetEnvironmentVariable("TRIM_END_VALUES"));
        }

        [TestMethod]
        public void Parse_WhenDisabledTrimStartKeys_ShouldNotIgnoreLeadingWhitepaces()
        {
            string env = "   TRIM_START_KEYS=1";

            new EnvParser()
                .DisableTrimStartKeys()
                .Parse(env);

            Assert.AreEqual("1", GetEnvironmentVariable("   TRIM_START_KEYS"));
        }

        [TestMethod]
        public void Parse_WhenDisabledTrimEndKeys_ShouldNotIgnoreTrailingWhitepaces()
        {
            string env = "TRIM_END_KEYS   =1";

            new EnvParser()
                .DisableTrimEndKeys()
                .Parse(env);

            Assert.AreEqual("1", GetEnvironmentVariable("TRIM_END_KEYS   "));
        }

        [TestMethod]
        public void Parse_WhenDisabledTrimStartComments_ShouldNotIgnoreLeadingWhitepaces()
        {
            string env = "   #comment1";

            Action action = () =>
            {
                new EnvParser()
                    .DisableTrimStartComments()
                    .Parse(env);
            };

            var ex = Assert.ThrowsException<ParserException>(action);
            StringAssert.Contains(ex.Message, ExceptionMessages.LineHasNoKeyValuePairMessage);
        }

        [TestMethod]
        public void Parse_WhenAllowedOverwriteExistingVars_ShouldOverwriteTheValue()
        {
            string env = @"
                ALLOW_OVERWRITE_1 = VAL1
                ALLOW_OVERWRITE_2 = VAL2
            ";
            SetEnvironmentVariable("ALLOW_OVERWRITE_1", "1");
            SetEnvironmentVariable("ALLOW_OVERWRITE_2", "2");

            new EnvParser()
                .AllowOverwriteExistingVars()
                .Parse(env);

            Assert.AreEqual("VAL1", GetEnvironmentVariable("ALLOW_OVERWRITE_1"));
            Assert.AreEqual("VAL2", GetEnvironmentVariable("ALLOW_OVERWRITE_2"));
        }

        [TestMethod]
        public void Parse_WhenSetsCommentChar_ShouldIgnoreComment()
        {
            string env = @"
                ;KEY1 = VAL1
                ;KEY2 = VAL2
            ";

            new EnvParser()
                .SetCommentChar(';')
                .Parse(env);

            Assert.IsNull(GetEnvironmentVariable(";KEY1"));
            Assert.IsNull(GetEnvironmentVariable(";KEY2"));
        }
    }
}
