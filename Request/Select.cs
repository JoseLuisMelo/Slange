using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Slange.Sql
{
    /// <summary>
    /// Intrucción Sql del tipo Select
    /// </summary>
    public class Select : Request
    {
        /// <summary>
        /// Crea una instrucción Sql
        /// </summary>
        /// <param name="Statement">Enunciado Sql</param>
        /// <returns></returns>
        public Select(string Statement) : base(Statement)
        {
        }

        /// <summary>
        /// Crea una instrucción Sql
        /// </summary>
        /// <param name="Statement">Enunciado Sql</param>
        /// <param name="Parameters">Parametros de variables Sql</param>
        /// <returns></returns>
        public Select(string Statement, List<SqlParameter> Parameters) : base(Statement, Parameters)
        {
        }

        /// <summary>
        /// Crea una instrucción Sql
        /// </summary>
        /// <param name="Statement">Enunciado Sql</param>
        /// <param name="Parameters">Parametros de variables Sql</param>
        /// <param name="IsSotoreProcedure">Indica si la instrucción corresponde a un StoreProcedura (default <strong>False</strong>)</param>
        /// <returns></returns>
        public Select(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure) : base(Statement, Parameters, IsSotoreProcedure)
        {
        }
    }
}
