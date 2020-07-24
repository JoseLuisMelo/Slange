using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Crud
{
    public interface IDelete
    {
        public DeleteAction Delete { get; }
    }

    public class DeleteAction
    {
        private string ModelName { get; }

        public DeleteAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        public Delete Where(string where)
        {
            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Delete($"DELETE FROM {ModelName} WHERE {where}");
        }
    }
}
