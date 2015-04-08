def context_template name
return <<template
using System;
using #{@solution_name_sans_extension}.Models;
using Needletail.DataAccess;

namespace #{@solution_name_sans_extension}.Context
{
    public class #{name}Context: DBTableDataSourceBase<#{name}, int>
    {
        public #{name}Context(string connectionString): base(connectionString, "#{name}s")
        {
        }
    }
}
template
end