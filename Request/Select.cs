using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Slange.Sql
{
    public class Select : Request
    {
        public Select(string Statement) : base(Statement)
        {
        }

        public Select(string Statement, List<SqlParameter> Parameters) : base(Statement, Parameters)
        {
        }

        public Select(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure) : base(Statement, Parameters, IsSotoreProcedure)
        {
        }
    }
}
