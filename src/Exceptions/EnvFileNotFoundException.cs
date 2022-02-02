using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEnv.Core
{
    internal class EnvFileNotFoundException : FileNotFoundException
    {
        /// <summary>
        /// Formats an error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns></returns>
        public static string FormatErrorMessage(string message, string fileName)
            => $"{message} (FileName: {fileName})";
    }
}
