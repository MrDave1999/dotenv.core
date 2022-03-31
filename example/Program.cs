using System;

namespace DotEnv.Core.Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var reader = new EnvReader();

			Console.WriteLine("---- EXAMPLE (1):");
			new EnvLoader()
				.EnableFileNotFoundException()
				.Load();
			Console.WriteLine($"MYSQL_HOST={reader["MYSQL_HOST"]}");
			Console.WriteLine($"MYSQL_DB={reader["MYSQL_DB"]}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (2):");
			new EnvLoader()
				.SetBasePath("files")
				.Load();
			Console.WriteLine($"MARIADB_HOST={reader["MARIADB_HOST"]}");
			Console.WriteLine($"MARIADB_DB={reader["MARIADB_DB"]}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (3):");
			new EnvLoader()
				.AddEnvFile("./files/.env.example")
				.Load();
			Console.WriteLine($"MONGODB_HOST={reader["MONGODB_HOST"]}");
			Console.WriteLine($"MONGODB_DB={reader["MONGODB_DB"]}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (4):");
			new EnvLoader()
				.AddEnvFiles("files/sqlite")
				.EnableFileNotFoundException()
				.Load();
			Console.WriteLine($"SQLITE_HOST={reader["SQLITE_HOST"]}");
			Console.WriteLine($"SQLITE_DB={reader["SQLITE_DB"]}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (5):");
			new EnvLoader()
				.SetDefaultEnvFileName(".env.local")
				.SetBasePath("./files/pgsql")
				.Load();
			Console.WriteLine($"PGSQL_HOST={reader["PGSQL_HOST"]}");
			Console.WriteLine($"PGSQL_DB={reader["PGSQL_DB"]}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (6):");
			EnvValidationResult result;
			new EnvLoader()
				.IgnoreParserException()
				.SetDefaultEnvFileName("./files/.env.local")
				.Load(out result);

			Console.WriteLine($"API_KEY LENGTH={reader["API_KEY"].Length}");
			Console.WriteLine(result.ErrorMessages);
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (7):");
			var keyValuePairs = new EnvLoader()
						.AvoidModifyEnvironment()
						.AddEnvFiles("./", "./files/", "files/sqlite", "./files/pgsql/.env.local")
						.Load();
			Console.WriteLine("-> Dictionary<string, string>:");
			foreach (var keyValuePair in keyValuePairs.ToDictionary())
				Console.WriteLine($"{keyValuePair.Key}, {keyValuePair.Value}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (8):");
			new EnvLoader()
				.SetBasePath("./files/environment")
				.AllowConcatDuplicateKeys()
				.LoadEnv(out result);
			if (result.HasError())
				Console.WriteLine(result.ErrorMessages);
			else
				Console.WriteLine($"CONNECTION_STRING_MSSQL={reader["CONNECTION_STRING_MSSQL"]}");
			Console.WriteLine("\n\n\n");

			Console.WriteLine("---- EXAMPLE (9):");
			keyValuePairs = new EnvLoader()
					.AllowConcatDuplicateKeys()
					.SetBasePath("./files/environment")
					.AvoidModifyEnvironment()
					.LoadEnv(out result);

			if(result.HasError())
				Console.WriteLine(result.ErrorMessages);
			else
            {
				foreach(var keyValuePair in keyValuePairs)
					Console.WriteLine($"{keyValuePair.Key}, {keyValuePair.Value}");
			}
			Console.WriteLine("\n\n\n");

			Console.Write("---- EXAMPLE (10):");
			new EnvValidator()
				.SetRequiredKeys("SERVICE_ID", "TOKEN_ID")
				.IgnoreException()
				.Validate(out result);

			Console.Write(result.ErrorMessages);
			Console.WriteLine("\n\n\n");
		}
	}
}
