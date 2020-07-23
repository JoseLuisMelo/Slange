using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Slange
{
    public static class Tools
    {
        #region Sql Connection
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
        public static bool ExecBool(this Insert Request, SqlConnection Connection) => Db.Exec.BoolRequest(Request, Connection);
        public static bool ExecBool(this Delete Request, SqlConnection Connection) => Db.Exec.BoolRequest(Request, Connection);
        public static bool ExecBool(this Update Request, SqlConnection Connection) => Db.Exec.BoolRequest(Request, Connection);

        public static int ExecInt(this Insert Request, SqlConnection Connection) => Db.Exec.IntRequest(Request, Connection);
        public static int ExecInt(this Delete Request, SqlConnection Connection) => Db.Exec.IntRequest(Request, Connection);
        public static int ExecInt(this Update Request, SqlConnection Connection) => Db.Exec.IntRequest(Request, Connection);

        public static DataTable ExecTable(this Select Request, SqlConnection Connection) => Db.Exec.TableRequest(Request, Connection);
        public static DataSet ExecDataSet(this Select Request, SqlConnection Connection) => Db.Exec.DataSetRequest(Request, Connection);
        #endregion

        #region Model
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
                    throw new Exception("You added a property that not exists in the origin model");
            });

            return output.ToArray();
        }
        #endregion

        #region Data
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
