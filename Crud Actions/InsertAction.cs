using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slange.Crud
{
    public interface IInsert<T> where T : class, new()
    {
        public InsertAction<T> Insert { get; }
    }

    public class InsertAction<T> where T: class, new()
    {
        private string ModelName { get; }

        public InsertAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        public virtual Insert Save<TOut>(Func<T, TOut> filter, T take) where TOut: class
        {
            var properties = Tools.FilterProperties(filter).ToList();

            if (take is null)
                throw new Exception("You must use not-null object");

            var columns = new List<string>();
            var values = new List<string>();
            var parameters = new List<SqlParameter>();

            properties.ForEach(prop =>
            {
                columns.Add(prop.Name);
                values.Add($"@{prop.Name}");

                var value = prop.GetValue(take) is null ? DBNull.Value : prop.GetValue(take);

                parameters.Add(new SqlParameter($"@{prop.Name}", value));
            });

            var insert = new Insert(
                $"INSERT INTO {ModelName} ({string.Join(',', columns)}) VALUES ({string.Join(',', values)})",
                parameters);

            return insert;
        }
    }
}
