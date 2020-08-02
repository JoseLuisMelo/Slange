using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Slange
{
    /// <summary>
    /// Define un conjunto de metodos de ayuda generales para la libreria
    /// </summary>
    public static class Tools
    {
        #region Sql Connection
        /// <summary>
        /// Verifica el estatus de una conexión Sql
        /// </summary>
        /// <param name="Connection"></param>
        public static void Check(this SqlConnection Connection)
        {
            try
            {
                if (Connection.State == ConnectionState.Executing)
                    throw new Exception("LA CONEXIÓN SE ENCUENTRA EJECUTANDO OTRA TRANSACCIÓN");
                if (Connection.State == ConnectionState.Fetching)
                    throw new Exception("LA CONEXÍON SE ENCUENTRA RECUPERANDO DATOS");
                if (Connection.State == ConnectionState.Connecting)
                    throw new Exception("LA CONEXIÓN ESTÁ TRATANDO DE CONECTARSE AL MOTOR DE BASE DE DATOS");
                else
                {
                    Connection.Close();
                    Connection.Open();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToUpper());
            }
        }
        #endregion

        #region CRUD actions
        /// <summary>
        /// Ejecuta una instrucción Sql del tipo INSERT
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>
        /// <para><strong>True: </strong> Si la instrucción Sql afectó a más de una fila en una tabla o si el Store Procedure se ejecutó correctamente</para>
        /// <para><strong>False: </strong> Si la instrucción Sql no afectó ninguna fila en una tabla o si el Store Procedure se ejecutó con error</para>
        /// </returns>
        public static bool ExecBool(this Insert Request, SqlConnection Connection) => Db.Exec.BoolRequest(Request, Connection);

        /// <summary>
        /// Ejecuta una instrucción Sql del tipo DELELE
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>
        /// <para><strong>True: </strong> Si la instrucción Sql afectó a más de una fila en una tabla o si el Store Procedure se ejecutó correctamente</para>
        /// <para><strong>False: </strong> Si la instrucción Sql no afectó ninguna fila en una tabla o si el Store Procedure se ejecutó con error</para>
        /// </returns>
        public static bool ExecBool(this Delete Request, SqlConnection Connection) => Db.Exec.BoolRequest(Request, Connection);

        /// <summary>
        /// Ejecuta una instrucción Sql del tipo UPDATE
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>
        /// <para><strong>True: </strong> Si la instrucción Sql afectó a más de una fila en una tabla o si el Store Procedure se ejecutó correctamente</para>
        /// <para><strong>False: </strong> Si la instrucción Sql no afectó ninguna fila en una tabla o si el Store Procedure se ejecutó con error</para>
        /// </returns>
        public static bool ExecBool(this Update Request, SqlConnection Connection) => Db.Exec.BoolRequest(Request, Connection);

        /// <summary>   
        /// Ejecuta una instrucción Sql del tipo INSERT
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>
        /// <para>Numero de filas afectadas por la instrucción Sql (-1 para Store Procedures sin consultas)</para>
        /// </returns>
        public static int ExecInt(this Insert Request, SqlConnection Connection) => Db.Exec.IntRequest(Request, Connection);

        /// <summary>   
        /// Ejecuta una instrucción Sql del tipo DELETE
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>
        /// <para>Numero de filas afectadas por la instrucción Sql (-1 para Store Procedures sin consultas)</para>
        /// </returns>
        public static int ExecInt(this Delete Request, SqlConnection Connection) => Db.Exec.IntRequest(Request, Connection);

        /// <summary>   
        /// Ejecuta una instrucción Sql del tipo UPDATE
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>
        /// <para>Numero de filas afectadas por la instrucción Sql (-1 para Store Procedures sin consultas)</para>
        /// </returns>
        public static int ExecInt(this Update Request, SqlConnection Connection) => Db.Exec.IntRequest(Request, Connection);

        /// <summary>   
        /// Ejecuta una instrucción Sql del tipo SELECT
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>DataTable resultante</returns>
        public static DataTable ExecTable(this Select Request, SqlConnection Connection) => Db.Exec.TableRequest(Request, Connection);

        /// <summary>   
        /// Ejecuta una instrucción Sql del tipo SELECT
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Connection"></param>
        /// <returns>DataSet resultante (util cuando se requiere obtener mas de una tabla a la vez)</returns>
        public static DataSet ExecDataSet(this Select Request, SqlConnection Connection) => Db.Exec.DataSetRequest(Request, Connection);
        #endregion

        #region Model
        /// <summary>
        /// A partir de una funcion lambda, esta funcion te permite obtener el detalle de un cierto grupo de propiedades pertenecientes a un objeto
        /// </summary>
        /// <param name="filter">Filtro de propiedades</param>
        /// <typeparam name="T">Tipo de dato de entrada (una clase)</typeparam>
        /// <typeparam name="TOut">Tipo de dato de salida (un <strong>objeto anonimo</strong>)</typeparam>
        /// <returns>Lista de detalle de las propiedades solicitadas</returns>
        public static PropertyInfo[] FilterProperties<T, TOut>(Func<T, TOut> filter)
        where T : class, new()
        where TOut : class
        {
            var output = new List<PropertyInfo>();

            T from = Activator.CreateInstance<T>();
            var properties = filter.Invoke(from).GetType().GetProperties().ToList();

            properties.ForEach(prop =>
            {
                bool contains = from.GetType().GetProperty(prop.Name) != null;
                bool exists = output.Exists(x => x.Name == prop.Name);

                if (contains && !exists)
                    output.Add(prop);
                else
                    throw new Exception("You added a property that not exists in the original object");
            });

            return output.ToArray();
        }
        #endregion

        #region Data
        /// <summary>
        /// Convierte un DataTable a una lista de objetos
        /// </summary>
        /// <typeparam name="T">Tipo de dato de salida</typeparam>
        /// <returns>Lista de objetos</returns>
        public static List<T> ToList<T>(this DataTable source) where T : class
        {
            try
            {
                if (source is null || source.Rows.Count == 0)
                    throw new Exception("The data source can't be null or empty".ToUpper());

                List<T> output = new List<T>();

                foreach (DataRow row in source.Rows)
                {
                    T item = Activator.CreateInstance<T>();

                    foreach (DataColumn column in source.Columns)
                    {
                        var exists_in = typeof(T).GetProperty(column.ColumnName) != null;

                        if (exists_in)
                        {
                            var value = row[column.ColumnName];

                            if (value == DBNull.Value)
                                value = null;

                            typeof(T).GetProperty(column.ColumnName).SetValue(item, value);
                        }
                    }

                    output.Add(item);
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToUpper());
            }
        }

        /// <summary>
        /// Obtiene el valor de la celda en la posicion [0,0] de una data table,
        /// esta funcion resulta particularmente útil para consultas del tipo COUNT donde el resultado 
        /// de esta consulta es una tabla de una sola fila y una sola columna
        /// </summary>
        /// <param name="source">Fuente de datos</param>
        /// <typeparam name="T">Tipo de dato esperado</typeparam>
        /// <returns>Valor esperado</returns>
        public static T FirstCell<T>(this DataTable source)
        {
            try
            {
                if (source is null || source.Rows.Count == 0)
                    throw new Exception("The data source can't be null or empty".ToUpper());

                return (T)Convert.ChangeType(source.Rows[0][0], typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToUpper());
            }
        }
        #endregion
    }
}
