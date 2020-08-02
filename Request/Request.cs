using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Sql
{
    /// <summary>
    /// Intrucción Sql <strong>Generica</strong>
    /// </summary>
    public abstract class Request
    {
        /// <summary>
        /// Enunciado Sql
        /// </summary>
        /// <value></value>
        public string Statement { get; }

        /// <summary>
        /// Parametros Sql
        /// </summary>
        /// <value></value>
        public List<SqlParameter> Parameters { get; }

        /// <summary>
        /// Especifica si la solicitud Sql es un Store Procedure
        /// </summary>
        /// <value></value>
        public bool IsSotoreProcedure { get; }

        /// <summary>
        /// Crea una instrucción Sql
        /// </summary>
        /// <param name="Statement">Enunciado Sql</param>
        /// <returns></returns>
        public Request(string Statement)
        {
            this.Statement = Statement;
            this.Parameters = new List<SqlParameter>();
            this.IsSotoreProcedure = false;
        }

        /// <summary>
        /// Crea una instrucción Sql
        /// </summary>
        /// <param name="Statement">Enunciado Sql</param>
        /// <param name="Parameters">Parametros de variables Sql</param>
        /// <returns></returns>
        public Request(string Statement, List<SqlParameter> Parameters)
        {
            this.Statement = Statement;
            this.Parameters = Parameters is null ? new List<SqlParameter>() : Parameters;
            this.IsSotoreProcedure = false;
        }

        /// <summary>
        /// Crea una instrucción Sql
        /// </summary>
        /// <param name="Statement">Enunciado Sql</param>
        /// <param name="Parameters">Parametros de variables Sql</param>
        /// <param name="IsSotoreProcedure">Indica si la instrucción corresponde a un StoreProcedura (default <strong>False</strong>)</param>
        /// <returns></returns>
        public Request(string Statement, List<SqlParameter> Parameters, bool IsSotoreProcedure)
        {
            this.Statement = Statement;
            this.Parameters = Parameters is null ? new List<SqlParameter>() : Parameters;
            this.IsSotoreProcedure = IsSotoreProcedure;
        }
    }
}
