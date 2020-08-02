using Microsoft.Data.SqlClient;
using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slange.Crud
{
    /// <summary>
    /// Implementa la función <strong>INSERT</strong> en un contexto de datos para un modelo
    /// </summary>
    public interface IInsert<T> where T : class, new()
    {
        /// <summary>
        /// Acción INSERT
        /// </summary>
        /// <value></value>
        public InsertAction<T> Insert { get; }
    }

    /// <summary>
    /// Definición de la acción INSERT 
    /// </summary>
    public class InsertAction<T> where T : class, new()
    {
        /// <summary>
        /// Nombre del modelo utilizado
        /// </summary>
        /// <value></value>
        private string ModelName { get; }

        /// <summary>
        /// Crea una nueva acción INSERT 
        /// </summary>
        /// <param name="ModelName"></param>
        public InsertAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        /// <summary>
        /// Inserta un registro
        /// </summary>
        /// <param name="filter">Columnas a insertar</param>
        /// <param name="take">Fuente de los datos a insertar</param>
        public virtual Insert Save<TOut>(Func<T, TOut> filter, T take) where TOut : class
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
