using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Migrations.Model
{
    internal class MigrationContext : Needletail.DataAccess.DBTableDataSourceBase<Migration, Guid>
    {
        public MigrationContext(string connetionString)
            : base(connetionString, "Migration")
        {
        }
    }
}
