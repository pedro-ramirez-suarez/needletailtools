using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Models
{
    public class Schema
    {

        public string column_name { get; set; }
        public string Data_type { get; set; }
        public int ordinal_position { get; set; }
        public string IS_NULLABLE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string CONSTRAINT_NAME { get; set; }
    }
}
