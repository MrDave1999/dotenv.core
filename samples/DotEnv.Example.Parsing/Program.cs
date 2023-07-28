using DotEnv.Core;

var source = 
"""
 DB_HOST=localhost
 DB_USERNAME=mrdave1999
 DB_PASSWORD=123456789$
""";
var envVars = new EnvParser().Parse(source);
Console.WriteLine("DB_HOST="     + envVars["DB_HOST"]);
Console.WriteLine("DB_USERNAME=" + envVars["DB_USERNAME"]);
Console.WriteLine("DB_PASSWORD=" + envVars["DB_PASSWORD"]);