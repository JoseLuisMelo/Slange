using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Slange.Sql
{
    public class Insert : Request
    {
        public Insert(string Statement) : base(Statement)
        {
        }

        public Insert(string Statement, List<SqlParameter> Parameters) : base(Statement, Parameters)
        {
        }

        public Insert(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure) : base(Statement, Parameters, IsSotoreProcedure)
        {
        }
    }
}
