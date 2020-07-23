using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Slange.Sql
{
    public class Update : Request
    {
        public Update(string Statement) : base(Statement)
        {
        }

        public Update(string Statement, List<SqlParameter> Parameters) : base(Statement, Parameters)
        {
        }

        public Update(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure) : base(Statement, Parameters, IsSotoreProcedure)
        {
        }
    }
}
