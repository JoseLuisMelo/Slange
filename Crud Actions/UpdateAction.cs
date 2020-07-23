using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slange.Crud
{
    public interface IUpdate<T> where T : class, new()
    {
        public UpdateAction<T> Update { get; }
    }

    public class UpdateAction<T> where T: class, new()
    {
        private string ModelName { get; }

        public UpdateAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        public Update Save<TOut>(Func<T, TOut> filter, T take, string where) where TOut: class
        {
            var properties = Tools.FilterProperties(filter).ToList();

            if (take is null)
                throw new Exception("You must use not-null object");

            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");
            if (!where.Substring(0, 6).ToUpper().Equals("WHERE "))
                throw new Exception("You must use a 'where' key word");


            var set = new List<string>();
            var parameters = new List<SqlParameter>();

            properties.ForEach(prop =>
            {
                set.Add($"{prop.Name} = @{prop.Name}");

                object value = take.GetType().GetProperty(prop.Name).GetValue(take);

                value = value is null ? DBNull.Value : value;

                parameters.Add(new SqlParameter($"@{prop.Name}", value));
            });

            var update = new Update(
                $"UPDATE {ModelName} SET {string.Join(',', set)} {where}",
                parameters);

            return update;
        }
    }
}
