using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Sql
{
    public class Request
    {
        public string Statement { get; }
        public List<SqlParameter> Parameters { get; }
        public bool IsSotoreProcedure { get; }

        public Request(string Statement)
        {
            this.Statement = Statement;
            this.Parameters = new List<SqlParameter>();
            this.IsSotoreProcedure = false;
        }

        public Request(string Statement, List<SqlParameter> Parameters)
        {
            this.Statement = Statement;
            this.Parameters = Parameters is null ? new List<SqlParameter>() : Parameters;
            this.IsSotoreProcedure = false;
        }

        public Request(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure)
        {
            this.Statement = Statement;
            this.Parameters = Parameters is null ? new List<SqlParameter>() : Parameters;
            this.IsSotoreProcedure = IsSotoreProcedure;
        }
    }
}
