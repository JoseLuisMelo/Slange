using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;

/// <summary>
/// Estas clases tienen como propósito poder des serializar el un archivo de configuracion json (appsettings)
/// El archivo de configuración debe contener de manera FORZOSA una propiedad "Application" la cual a su vez
/// contendrá otra propiedad llamada "ConnectionString" la cuál se encargará de almacenar la cadena de conexión que usará nuestro sistema
/// </summary>
namespace Slange.Sql
{
    public class Configuration
    {
        public ApplicationSettings Application { get; set; }
    }

    public class ApplicationSettings
    {
        public string ConnectionString { get; set; }
    }
}
