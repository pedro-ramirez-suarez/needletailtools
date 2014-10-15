using DataAccess.Scaffold.Attributes;
using Needletail.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needletail.SampleModel.Model.Entity
{
   public  class Address
    {
       [TableKey(CanInsertKey = true)]
       public Guid Id { get; set; }
       [MaxLen(150)]
       public string Street { get; set; }

       [ZipCode]
       public string ZipCode { get; set; }
       [Phone]
       public string Phone { get; set; }
    }
}
