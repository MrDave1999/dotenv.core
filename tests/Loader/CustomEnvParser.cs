using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core.Tests.Loader
{
    class CustomEnvParser : EnvParser
    {
        protected override bool IsComment(string line)
            => base.IsComment(line);

        protected override string ExtractKey(string line)
            => base.ExtractKey(line);

        protected override string ExtractValue(string line)
            => base.ExtractValue(line);

        protected override bool HasNoKeyValuePair(string line)
            => base.HasNoKeyValuePair(line);

        protected override void SetEnvironmentVariable(string key, string value)
            => base.SetEnvironmentVariable(key, value);
    }
}
