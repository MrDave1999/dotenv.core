﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotEnv.Core.Tests.Exceptions
{
    [TestClass]
    public class ParserExceptionTests
    {
        [TestMethod]
        public void Message_WhenPassingActualValueAndCurrentLine_ShouldReturnMessageWithActualValueAndCurrentLine()
        {
            var parser = new ParserException("This is a Message.", actualValue: 1, currentLine: 1);
            string expected = "This is a Message. (Actual Value: 1, Line: 1)";

            string actual = parser.Message;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Message_WhenPassingActualValue_ShouldReturnMessageWithActualValue()
        {
            var parser = new ParserException("This is a Message.", actualValue: 1);
            string expected = "This is a Message. (Actual Value: 1)";

            string actual = parser.Message;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Message_WhenPassingCurrentLine_ShouldReturnMessageWithCurrentLine()
        {
            var parser = new ParserException("This is a Message.", currentLine: 1);
            string expected = "This is a Message. (Line: 1)";

            string actual = parser.Message;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Message_WhenNotPassingActualValueAndCurrentLine_ShouldReturnMessage()
        {
            var parser = new ParserException("This is a Message.");
            string expected = "This is a Message.";

            string actual = parser.Message;

            Assert.AreEqual(expected, actual);
        }
    }
}