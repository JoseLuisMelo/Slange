using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Crud
{
    /// <summary>
    /// Implementa la función <strong>SELECT</strong> en un contexto de datos para un modelo
    /// </summary>
    public interface ISelect<T> where T : class, new()
    {
        /// <summary>
        /// Acción SELECT
        /// </summary>
        /// <value></value>
        public SelectAction<T> Select { get; }
    }

    /// <summary>
    /// Definición de la acción SELECT 
    /// </summary>
    public class SelectAction<T> where T : class, new()
    {
        /// <summary>
        /// Nombre del modelo utilizado
        /// </summary>
        /// <value></value>
        private string ModelName { get; }

        /// <summary>
        /// Crea una nueva acción SELECT 
        /// </summary>
        /// <param name="ModelName"></param>
        public SelectAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        /// <summary>
        /// Consulta un conjunto de registros de una tabla
        /// </summary>
        /// <param name="condition">Condición que deben cumplir los datos</param>
        /// <returns></returns>
        public Select Where(string condition)
        {
            if (condition is null || condition.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Select($"SELECT * FROM {ModelName} WHERE {condition}");
        }

        /// <summary>
        /// Coonsulta todos los datos de una tabla
        /// </summary>
        /// <returns></returns>
        public Select All()
        {
            return new Select($"SELECT * FROM {ModelName}");
        }

        /// <summary>
        /// Consulta un TOP de datos de una tabla
        /// </summary>
        /// <param name="top">Número de datos</param>
        /// <param name="where">Condición que deben cumplir los datos</param>
        /// <returns></returns>
        public Select Top(int top, string where)
        {
            if (top == 0)
                throw new Exception("You must to use a top > 0");
            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Select($"SELECT TOP {top} * FROM {ModelName} WHERE {where}");
        }

        /// <summary>
        /// Cuenta un número determinado de datos
        /// </summary>
        /// <param name="where">Condición que deben cumplir los datos</param>
        /// <returns></returns>
        public Select Count(string where)
        {
            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Select($"SELECT COUNT(*) FROM {ModelName} WHERE {where}");

        }
    }
}
