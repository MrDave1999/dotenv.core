using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotEnv.Core.ExceptionMessages;
using static System.Environment;

namespace DotEnv.Core.Tests.Parser
{
    [TestClass]
    public class EnvParserTests
    {
        [TestMethod]
        [DataRow("")]
        [DataRow("       ")]
        public void Parse_WhenInputIsEmptyOrWhitespace_ShouldThrowParserException(string input)
        {
            var parser = new EnvParser();

            Action action = () => parser.Parse(input);

            var ex = Assert.ThrowsException<ParserException>(action);
            StringAssert.Contains(ex.Message, InputIsEmptyOrWhitespaceMessage);
        }

        [TestMethod]
        public void Parse_WhenKeyIsAnEmptyString_ShouldThrowParserException()
        {
            string env = @"
                KEY_EMPTY_STRING=VAL1
                =VAL2
            ";
            var parser = new EnvParser();

            Action action = () => parser.Parse(env);

            var ex = Assert.ThrowsException<ParserException>(action);
            StringAssert.Contains(ex.Message, KeyIsAnEmptyStringMessage);
        }

        [TestMethod]
        [DataRow(@"
            LINE_HAS_NO_KEY_1=VAL1
            LINE_HAS_NO_KEY_2;VAL2
        ")]
        [DataRow(@"
            LINE_HAS_NO_KEY_3 VAL1
            LINE_HAS_NO_KEY_4=VAL2
        ")]
        [DataRow(@"
            This is a line.
            LINE_HAS_NO_KEY_5=VAL2
        ")]
        public void Parse_WhenLineHasNoKeyValuePair_ShouldThrowParserException(string input)
        {
            var parser = new EnvParser();

            Action action = () => parser.Parse(input);

            var ex = Assert.ThrowsException<ParserException>(action);
            StringAssert.Contains(ex.Message, LineHasNoKeyValuePairMessage);
        }

        [TestMethod]
        public void Parse_WhenReadComment_ShouldIgnoreComment()
        {
            string env = @"
                #KEY1=VAL1
                   #KEY2=VAL2
                        #KEY3=VAL3
                #KEY4=VAL4
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.IsNull(GetEnvironmentVariable("#KEY1"));
            Assert.IsNull(GetEnvironmentVariable("#KEY2"));
            Assert.IsNull(GetEnvironmentVariable("#KEY3"));
            Assert.IsNull(GetEnvironmentVariable("#KEY4"));
        }

        [TestMethod]
        public void Parse_WhenReadLineWithKeyValuePair_ShouldExtractKey()
        {
            string env = @"
                READLINE_WITH_KEY_1=VAL1
                READLINE_WITH_KEY_2=VAL2
                READLINE_WITH_KEY_3=VAL3
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.IsNotNull(GetEnvironmentVariable("READLINE_WITH_KEY_1"));
            Assert.IsNotNull(GetEnvironmentVariable("READLINE_WITH_KEY_2"));
            Assert.IsNotNull(GetEnvironmentVariable("READLINE_WITH_KEY_3"));
        }

        [TestMethod]
        public void Parse_WhenReadLineWithKeyValuePair_ShouldExtractValue()
        {
            string env = @"
                READLINE_WITH_VALUE_1=VAL1
                READLINE_WITH_VALUE_2=VAL2
                READLINE_WITH_VALUE_3=VAL3
                READLINE_WITH_VALUE_4=server=localhost;user=root;password=1234;
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.AreEqual("VAL1", GetEnvironmentVariable("READLINE_WITH_VALUE_1"));
            Assert.AreEqual("VAL2", GetEnvironmentVariable("READLINE_WITH_VALUE_2"));
            Assert.AreEqual("VAL3", GetEnvironmentVariable("READLINE_WITH_VALUE_3"));
            Assert.AreEqual("server=localhost;user=root;password=1234;", GetEnvironmentVariable("READLINE_WITH_VALUE_4"));
        }

        [TestMethod]
        public void Parse_WhenValueIsEmptyString_ShouldConvertItToWhiteSpace()
        {
            string env = @"
                VALUE_EMPTY_STRING_1=
                VALUE_EMPTY_STRING_2=
                VALUE_EMPTY_STRING_3=
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.AreEqual(" ", GetEnvironmentVariable("VALUE_EMPTY_STRING_1"));
            Assert.AreEqual(" ", GetEnvironmentVariable("VALUE_EMPTY_STRING_2"));
            Assert.AreEqual(" ", GetEnvironmentVariable("VALUE_EMPTY_STRING_3"));
        }

        [TestMethod]
        public void Parse_WhenKeyHasWhitespacesAtBothEnds_ShouldIgnoreWhitespaces()
        {
            string env = @"  
                       KEY_HAS_WHITESPACES_1    =     VAL1               
                       KEY_HAS_WHITESPACES_2    =     VAL2             
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.IsNotNull(GetEnvironmentVariable("KEY_HAS_WHITESPACES_1"));
            Assert.IsNotNull(GetEnvironmentVariable("KEY_HAS_WHITESPACES_2"));
        }

        [TestMethod]
        public void Parse_WhenValueHasWhitespacesAtBothEnds_ShouldIgnoreWhitespaces()
        {
            string env = @"  
                       VALUE_HAS_WHITESPACES_1    =     VAL1               
                       VALUE_HAS_WHITESPACES_2    =     VAL2             
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.AreEqual("VAL1", GetEnvironmentVariable("VALUE_HAS_WHITESPACES_1"));
            Assert.AreEqual("VAL2", GetEnvironmentVariable("VALUE_HAS_WHITESPACES_2"));
        }

        [TestMethod]
        public void Parse_WhenKeyExistsInTheCurrentProcess_ShouldNotOverwriteTheValue()
        {
            string env = @"  
                       KEY_EXISTS_1  =     VAL1               
                       KEY_EXISTS_2  =     VAL2             
            ";
            var parser = new EnvParser();
            SetEnvironmentVariable("KEY_EXISTS_1", "1");
            SetEnvironmentVariable("KEY_EXISTS_2", "2");

            parser.Parse(env);

            Assert.AreEqual("1", GetEnvironmentVariable("KEY_EXISTS_1"));
            Assert.AreEqual("2", GetEnvironmentVariable("KEY_EXISTS_2"));
        }

        [TestMethod]
        public void Parse_WhenVariablesAreEmbeddedInTheValue_ShouldExpandVariables()
        {
            string env = @"
                MYSQL_USER_EXPAND = root
                EXPAND_1 = 1
                MYSQL_HOST_EXPAND = localhost
                MYSQL_PASSWORD_EXPAND = 1234
                CS_MYSQL_EXPAND = server=${MYSQL_HOST_EXPAND};user=${MYSQL_USER_EXPAND};password=${MYSQL_PASSWORD_EXPAND};
                CS_SQL_EXPAND = ${CS_MYSQL_EXPAND}
                EXPAND_2 = ${TEST asdasd
                EXPAND_3 = {TEST}$ $TEST {}
            ";
            var parser = new EnvParser();

            parser.Parse(env);

            Assert.AreEqual("server=localhost;user=root;password=1234;", GetEnvironmentVariable("CS_MYSQL_EXPAND"));
            Assert.AreEqual("server=localhost;user=root;password=1234;", GetEnvironmentVariable("CS_SQL_EXPAND"));
            Assert.AreEqual("${TEST asdasd", GetEnvironmentVariable("EXPAND_2"));
            Assert.AreEqual("{TEST}$ $TEST {}", GetEnvironmentVariable("EXPAND_3"));
        }

        [TestMethod]
        [DataRow("EMBEDDED_VAR_1 = asdasd ${EMBEDDED_VAR_1}")]
        [DataRow("EMBEDDED_VAR_2 = ${VARIABLE_NOT_FOUND} asdasd ")]
        public void Parse_WhenEmbeddedVariableDoesNotExist_ShouldThrowParserException(string env)
        {
            var parser = new EnvParser();

            Action action = () => parser.Parse(env);

            var ex = Assert.ThrowsException<ParserException>(action);
            StringAssert.Contains(ex.Message, VariableNotFoundMessage);
        }

        [TestMethod]
        [DataRow("EMBEDDED_VAR_3 = asdasd ${}")]
        [DataRow("EMBEDDED_VAR_4 = ${    } asdasd ")]
        public void Parse_WhenEmbeddedVariableIsEmptyString_ShouldThrowParserException(string env)
        {
            var parser = new EnvParser();

            Action action = () => parser.Parse(env);

            var ex = Assert.ThrowsException<ParserException>(action);
            StringAssert.Contains(ex.Message, VariableIsAnEmptyStringMessage);
        }
    }
}