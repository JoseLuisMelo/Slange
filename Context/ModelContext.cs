using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Context
{
    /// <summary>
    /// Define la clase base de un contexto de datos enfocado a un modelo
    /// </summary>
    public abstract class ModelContext
    {
        /// <summary>
        /// Nombre del modelo de datos
        /// </summary>
        /// <value></value>
        public string ModelName { get; }

        /// <summary>
        /// Inicializa el contexto del modelo
        /// </summary>
        /// <param name="ModelName"></param>
        public ModelContext(string ModelName)
        {
            this.ModelName = ModelName;
        }
    }
}
