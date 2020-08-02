using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Slange
{
    /// <summary>
    /// Clase encargada de la gestión de conexiónes con el motor de base de datos Sql
    /// </summary>
    public static class Connection
    {
        /// <summary>
        /// Cadena de conexión (obtenida a partir de la lectura del archivo de configuración correspondiente)
        /// </summary>
        /// <value></value>
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// Crea una nueva conexión Sql
        /// </summary>
        /// <returns>Conexión Sql</returns>
        public static SqlConnection NewConnection()
        {
            if (ConnectionString is null)
                throw new Exception("You must to set the connection string");

            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Captura el valor de la cadena de conexión Sql a partir de la lectura de un archivo de configuración (appsettings.json)
        /// </summary>
        /// <param name="settingsPath">Dirección del archivo de configuración</param>
        public static async void SetConnectionString(string settingsPath)
        {
            using FileStream fs = File.OpenRead(settingsPath);
            var configuration = await JsonSerializer.DeserializeAsync<Configuration>(fs);
            ConnectionString = configuration.Application.ConnectionString;
        }
    }
}
