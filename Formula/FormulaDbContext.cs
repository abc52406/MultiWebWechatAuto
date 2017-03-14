using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;
using Formula.Helper;
using System.Transactions;
using System.Reflection;

namespace Formula
{
    public class FormulaDbContext : DbContext
    {
        protected string ConnName = "";

        public FormulaDbContext(string connectionString)
            : base(connectionString)
        {
            ConnName = connectionString;
        }
    }
}
