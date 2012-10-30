using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;

namespace Needletail.TestProject.Model
{
    public class Task
    {
        [TableKey(CanInsertKey = true)]
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DueOn { get; set; }
    }

}
