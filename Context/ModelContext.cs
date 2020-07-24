﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slange.Context
{
    public abstract class ModelContext where T: class
    {
        public string ModelName { get; }

        public ModelContext(string ModelName)
        {
            this.ModelName = ModelName;
        }
    }
}
