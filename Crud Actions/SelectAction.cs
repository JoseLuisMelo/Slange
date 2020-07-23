using Slange.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Crud
{
    public interface ISelect<T> where T: class, new()
    {
        public SelectAction<T> Select { get; }
    }

    public class SelectAction<T> where T: class, new()
    {
        private string ModelName { get; }

        public SelectAction(string ModelName)
        {
            this.ModelName = ModelName;
        }

        public Select Where(string condition)
        {
            if (condition is null || condition.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Select($"SELECT * FROM {ModelName} WHERE {condition}");
        }


        public Select All()
        {
            return new Select($"SELECT * FROM {ModelName}");
        }

        public Select Top(int top, string where)
        {
            if (top == 0)
                throw new Exception("You must to use a top > 0");
            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Select($"SELECT TOP {top} * FROM {ModelName} WHERE {where}");
        }

        public Select Count(string where)
        {
            if (where is null || where.Equals(string.Empty))
                throw new Exception("You must include a 'where' condition");

            return new Select($"SELECT COUNT(*) FROM {ModelName} WHERE {where}");

        }
    }
}
