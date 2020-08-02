using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Slange.Sql
{
    /// <summary>
    /// Clase encargada de hacer transacciónes con el motor de base de datos
    /// </summary>
    public class Db : Actions
    {
        private Db() { }

        /// <summary>
        /// Manager de transacciónes Sql
        /// </summary>
        /// <returns></returns>
        public static Db Exec { get; } = new Db();

        /// <summary>
        /// Ejecuta una instrucción Sql
        /// </summary>
        /// <param name="Request">Instrucción Sql</param>
        /// <param name="Connection">Conexión Sql</param>
        /// <returns>
        /// <para><strong>True: </strong> Si la instrucción Sql afectó a más de una fila en una tabla o si el Store Procedure se ejecutó correctamente</para>
        /// <para><strong>False: </strong> Si la instrucción Sql no afectó ninguna fila en una tabla o si el Store Procedure se ejecutó con error</para>
        /// </returns>
        public bool BoolRequest(Request Request, SqlConnection Connection) => ExecuteNonQuery<bool>(Request, Connection);

        /// <summary>
        /// Ejecuta una instrucción Sql
        /// </summary>
        /// <param name="Request">Instrucción Sql</param>
        /// <param name="Connection">Conexión Sql</param>
        /// <returns>Número de filas afectadas</returns>
        public int IntRequest(Request Request, SqlConnection Connection) => ExecuteNonQuery<int>(Request, Connection);

        /// <summary>
        /// Ejecuta una instrucción Sql
        /// </summary>
        /// <param name="Request">Instrucción Sql</param>
        /// <param name="Connection">Conexión Sql</param>
        /// <returns>DataTable</returns>
        public DataTable TableRequest(Request Request, SqlConnection Connection) => ExecuteQuery<DataTable>(Request, Connection);

        /// <summary>
        /// Ejecuta una instrucción Sql
        /// </summary>
        /// <param name="Request">Instrucción Sql</param>
        /// <param name="Connection">Conexión Sql</param>
        /// <returns>DataSet</returns>
        public DataSet DataSetRequest(Request Request, SqlConnection Connection) => ExecuteQuery<DataSet>(Request, Connection);
    }
}
