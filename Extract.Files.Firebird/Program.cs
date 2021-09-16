using Extract.Files.Firebird;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.IO;
using System.Text.Json;

if (!File.Exists("config.json"))
{
    Console.WriteLine("Arquivo de configuração não foi encontrado!");
    Console.ReadLine();
    Environment.Exit(0);
}

string jsonString = File.ReadAllText("config.json");

Configuration? configuration = JsonSerializer.Deserialize<Configuration>(jsonString);

if (configuration is null)
{
    Console.WriteLine("Erro na configuração!");
    Console.ReadLine();
    Environment.Exit(0);
}

var connectionString = new FbConnectionStringBuilder
{
    Database = configuration.Database,
    UserID = configuration.User,
    Password = configuration.Password,
    DataSource = configuration.DataSource,
    Port = configuration.Port,
    Charset = configuration.Charset
}.ToString();


if (!Directory.Exists(configuration.PathOutput))
{
    Console.WriteLine("Diretório {0} não existe!", configuration.PathOutput);
    Console.ReadLine();
    Environment.Exit(0);
}

try
{
    using (var connection = new FbConnection(connectionString))
    {
        Console.WriteLine("######################");
        Console.WriteLine("Exportação iniciada!");
        Console.WriteLine("######################");

        connection.Open();
        using (var command = new FbCommand(configuration.Query, connection))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                string targetFile = Path.Combine(configuration.PathOutput, reader["ID"].ToString() + "." + configuration.Extension!);
                if (File.Exists(targetFile))
                {
                    Console.WriteLine("Arquivo {0} já existe...", targetFile);
                    continue;
                }

                using (var fs = new FileStream(targetFile, FileMode.Create))
                {
                    byte[] filedata = (byte[])reader["FILE"];
                    fs.Write(filedata, 0, filedata.Length);
                }

                Console.WriteLine("ID {0} exportado...", reader["ID"].ToString());
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine("Erro: " + e.Message);
}

Console.WriteLine("######################");
Console.WriteLine("Exportação finalizada!");
Console.WriteLine("######################");
Console.ReadLine();