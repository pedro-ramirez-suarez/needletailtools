using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;

namespace DataAccess.Migrations.Model
{
    public class Migration
    {
        [TableKey(CanInsertKey = true)]
        public Guid Id { get; set; }
        public string Script { get; set; }
        public DateTime ExecutedOn { get; set; }
    }
}
