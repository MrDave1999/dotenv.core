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
        public void Parse_WhenDisabledTrimValues_ShouldNotIgnoreWhitepaces()
        {
            string env = "TRIM_VALUES=   1   ";

            new EnvParser()
                .DisableTrimValues()
                .Parse(env);

            Assert.AreEqual("   1   ", GetEnvironmentVariable("TRIM_VALUES"));
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
        public void Parse_WhenDisabledTrimKeys_ShouldNotIgnoreWhitepaces()
        {
            string env = "   TRIM_KEYS   =1";

            new EnvParser()
                .DisableTrimKeys()
                .Parse(env);

            Assert.AreEqual("1", GetEnvironmentVariable("   TRIM_KEYS   "));
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

        [TestMethod]
        public void Parse_WhenSetsDelimiterKeyValuePair_ShouldReadVariables()
        {
            string env = @"
                DELIMITER_KEYVALUE_1 : VAL1
                DELIMITER_KEYVALUE_2:VAL2
            ";

            new EnvParser()
                .SetDelimiterKeyValuePair(':')
                .Parse(env);

            Assert.AreEqual("VAL1", GetEnvironmentVariable("DELIMITER_KEYVALUE_1"));
            Assert.AreEqual("VAL2", GetEnvironmentVariable("DELIMITER_KEYVALUE_2"));
        }

        [TestMethod]
        public void Parse_WhenIgnoresParserExceptions_ShouldNotThrowParserException()
        {
            string env = @"
                asdasdasdasd
                IGNORE_EXCEPTION_1=VAL1 ${IGNORE_EXCEPTION_2} ...
                KEY1:VAL1
                This isn't a comment.
                IGNORE_EXCEPTION_2=VAL2 ${IGNORE_EXCEPTION_2} ...
                =VAL1
            ";

            new EnvParser()
                .IgnoreParserExceptions()
                .Parse(env);

            Assert.IsNotNull(GetEnvironmentVariable("IGNORE_EXCEPTION_1"));
            Assert.IsNotNull(GetEnvironmentVariable("IGNORE_EXCEPTION_2"));
        }

        [TestMethod]
        public void Parse_WhenConcatDuplicateKeysAtEnd_ShouldReadDuplicateVariable()
        {
            string env = @"
                CONCAT_END2 = 1
                CONCAT_END1 = Hello
                CONCAT_END1 = World
                CONCAT_END1 = !
                CONCAT_END1 = !
            ";

            new EnvParser()
                .AllowConcatDuplicateKeys()
                .Parse(env);

            Assert.AreEqual("HelloWorld!!", GetEnvironmentVariable("CONCAT_END1"));
        }


        [TestMethod]
        public void Parse_WhenConcatDuplicateKeysAtStart_ShouldReadDuplicateVariable()
        {
            string env = @"
                CONCAT_START2 = 1
                CONCAT_START1 = !
                CONCAT_START1 = !
                CONCAT_START1 = World
                CONCAT_START1 = Hello
            ";

            new EnvParser()
                .AllowConcatDuplicateKeys(ConcatKeysOptions.Start)
                .Parse(env);

            Assert.AreEqual("HelloWorld!!", GetEnvironmentVariable("CONCAT_START1"));
        }
    }
}
