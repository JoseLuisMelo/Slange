using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;

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
