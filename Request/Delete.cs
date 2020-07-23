using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Slange.Sql
{
    public class Delete : Request
    {
        public Delete(string Statement) : base(Statement)
        {
        }

        public Delete(string Statement, List<SqlParameter> Parameters) : base(Statement, Parameters)
        {
        }

        public Delete(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure) : base(Statement, Parameters, IsSotoreProcedure)
        {
        }
    }
}
