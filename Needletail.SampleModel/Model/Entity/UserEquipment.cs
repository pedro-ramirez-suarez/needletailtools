using Needletail.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needletail.SampleModel.Model.Entity
{
    public class UserEquipment
    {
        [TableKey(CanInsertKey = true)]
        public Guid Id { get; set; }

        public string EquipmentName { get; set; }
        public decimal Price { get; set; }
        public Guid UserId { get; set; }
    }
}
