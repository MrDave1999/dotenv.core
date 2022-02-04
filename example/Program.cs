using System;

namespace DotEnv.Core.Example
{
	class CustomEnvParser : EnvParser
	{
		protected override string ExtractKey(string line)
		{
			string key = base.ExtractKey(line);
			Console.WriteLine($"CustomEnvParser.ExtractKey(string line) -> {key}");
			return key;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var reader = new EnvReader();

			new EnvLoader()
				.EnableFileNotFoundException()
				.Load();
			Console.WriteLine($"MYSQL_HOST={reader["MYSQL_HOST"]}");
			Console.WriteLine($"MYSQL_DB={reader["MYSQL_DB"]}");

			new EnvLoader()
				.SetBasePath("files")
				.Load();
			Console.WriteLine($"MARIADB_HOST={reader["MARIADB_HOST"]}");
			Console.WriteLine($"MARIADB_DB={reader["MARIADB_DB"]}");

			new EnvLoader()
				.AddEnvFile("./files/.env.example")
				.Load();
			Console.WriteLine($"MONGODB_HOST={reader["MONGODB_HOST"]}");
			Console.WriteLine($"MONGODB_DB={reader["MONGODB_DB"]}");

			new EnvLoader()
				.AddEnvFiles("files/sqlite")
				.EnableFileNotFoundException()
				.Load();
			Console.WriteLine($"SQLITE_HOST={reader["SQLITE_HOST"]}");
			Console.WriteLine($"SQLITE_DB={reader["SQLITE_DB"]}");

			new EnvLoader()
				.SetDefaultEnvFileName(".env.local")
				.SetBasePath("./files/pgsql")
				.Load();
			Console.WriteLine($"PGSQL_HOST={reader["PGSQL_HOST"]}");
			Console.WriteLine($"PGSQL_DB={reader["PGSQL_DB"]}");

			new EnvLoader(new CustomEnvParser())
				.DisableParserException()
				.SetDefaultEnvFileName("./files/.env.local")
				.Load(out var result);

			Console.WriteLine($"API_KEY LENGTH={reader["API_KEY"]}");
			Console.WriteLine(result.ErrorMessages);
		}
	}
}
