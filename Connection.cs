using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Slange
{
    public static class Connection
    {
        public static string ConnectionString { get; private set; }

        public static SqlConnection NewConnection()
        {
            if (ConnectionString is null)
                throw new Exception("You must to set the connection string");

            return new SqlConnection(ConnectionString);
        }

        public static async void SetConnectionString(string settingsPath)
        {
            using FileStream fs = File.OpenRead(settingsPath);
            var configuration = await JsonSerializer.DeserializeAsync<Configuration>(fs);
            ConnectionString = configuration.Application.ConnectionString;
        }
    }
}
