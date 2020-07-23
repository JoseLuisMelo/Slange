using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Slange.Sql
{
    public class Db: Actions
    {
        public static Db Exec { get; } = new Db();
        private Db() { }

        public bool BoolRequest(Request Request, SqlConnection Connection) => ExecuteNonQuery<bool>(Request, Connection);

        public int IntRequest(Request Request, SqlConnection Connection) => ExecuteNonQuery<int>(Request, Connection);

        public DataTable TableRequest(Request Request, SqlConnection Connection) => ExecuteQuery<DataTable>(Request, Connection);

        public DataSet DataSetRequest(Request Request, SqlConnection Connection) => ExecuteQuery<DataSet>(Request, Connection);
    }
}
