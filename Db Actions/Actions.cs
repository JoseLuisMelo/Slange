using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Slange.Sql
{
    public abstract class Actions
    {
        protected T ExecuteNonQuery<T>(Request request, SqlConnection connection)
        {
            if (connection.ConnectionString is null || connection.ConnectionString.Equals(string.Empty))
                connection.ConnectionString = Connection.ConnectionString;

            connection.Check();

            using (connection)
            {
                connection.Open();

                using SqlCommand command = connection.CreateCommand();

                if (request.IsSotoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;
                else
                    command.CommandType = CommandType.Text;

                if (request.Parameters != null)
                    command.Parameters.AddRange(request.Parameters.ToArray());

                command.CommandTimeout = 5000;

                using SqlTransaction transaction = connection.BeginTransaction("Slange");
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = request.Statement;

                try
                {
                    int rows = command.ExecuteNonQuery();
                    transaction.Commit();

                    T result = default;

                    if (rows == 0)
                        result = typeof(T) == typeof(bool) ? (T)Convert.ChangeType(false, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
                    else
                        result = typeof(T) == typeof(bool) ? (T)Convert.ChangeType(true, typeof(T)) : (T)Convert.ChangeType(rows, typeof(T));

                    return result;
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();

                        if (ex is SqlException sqlEx)
                        {
                            if (sqlEx.Number == 2627)
                                throw new Exception($"PK EXCEPTION: {ex.Message.ToUpper()}");

                            if (sqlEx.Number == 1025)
                                return ExecuteNonQuery<T>(request, connection);
                        }

                        throw new Exception($"STATEMENT WAS WRONG: {ex.Message.ToUpper()}");
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception($"ERROR (ROLLBACK/RETRY EXCEPTION): {ex2.Message.ToUpper()}");
                    }
                }
            }
        }

        protected static T ExecuteQuery<T>(Request request, SqlConnection connection)
        {
            if (connection.ConnectionString is null || connection.ConnectionString.Equals(string.Empty))
                connection.ConnectionString = Connection.ConnectionString;

            connection.Check();

            using (connection)
            {
                connection.Open();

                using SqlCommand command = connection.CreateCommand();

                if (request.IsSotoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;
                else
                    command.CommandType = CommandType.Text;

                if (request.Parameters != null)
                    command.Parameters.AddRange(request.Parameters.ToArray());

                command.CommandTimeout = 5000;

                command.Connection = connection;

                try
                {
                    command.CommandText = request.Statement;

                    T result = default;

                    if (typeof(T) == typeof(DataTable))
                    {
                        SqlDataReader sqlDataReader = command.ExecuteReader();
                        using (sqlDataReader)
                        {
                            var tbl = new DataTable();

                            tbl.Load(sqlDataReader);

                            result = (T)Convert.ChangeType(tbl, typeof(T));
                        };
                    }
                    else
                    {
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                        using (sqlDataAdapter)
                        {
                            var ds = new DataSet();

                            sqlDataAdapter.Fill(ds);

                            result = (T)Convert.ChangeType(ds, typeof(T));
                        };
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    if (ex is SqlException sqlEx)
                    {
                        if (sqlEx.Number == 2627)
                            throw new Exception($"PK EXCEPTION: {ex.Message.ToUpper()}");

                        if (sqlEx.Number == 1025)
                            return ExecuteQuery<T>(request, connection);
                    }

                    throw new Exception($"STATEMENT WAS WRONG: {ex.Message.ToUpper()}");
                }
            }
        }
    }
}
