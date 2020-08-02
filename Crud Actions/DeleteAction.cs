using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Crud
{
    /// <summary>
    /// Implementa la función <strong>DELETE</strong> en un contexto de datos para un modelo
    /// </summary>
    public interface IDelete
    {
        /// <summary>
        /// Acción DELETE
        /// </summary>
        /// <value></value>
        public DeleteAction Delete { get; }
    }

    /// <summary>
    /// Definición de la acción DELETE 
    /// </summary>
    public class DeleteAction
    {
        /// <summary>
        /// Nombre del modelo utilizado
        /// </summary>
        /// <value></value>
        private string ModelName { get; }

        /// <summary>
        /// Crea una nueva acción DELETE 
        /// </summary>
        /// <param name="ModelName"></param>
        public DeleteAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        /// <summary>
        /// Elimina un modelo de la base de datos
        /// </summary>
        /// <param name="where">Condición a cumplir (obligatoria)</param>
        /// <returns>Instrucción DELETE</returns>
        public Delete Where(string where)
        {
            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Delete($"DELETE FROM {ModelName} WHERE {where}");
        }
    }
}
