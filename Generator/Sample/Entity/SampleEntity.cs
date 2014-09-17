using DataAccess.Scaffold.Attributes;
using Needletail.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Sample.Entity
{
    public class SampleEntity
    {
        [TableKey(CanInsertKey = true)]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Email]
        public string Email { get; set; }

        [MaxLen(150)]
        public string Address { get; set; }

        public int Age { get; set; }

        public DateTime DateOfSubscription { get; set; }

    }
}
