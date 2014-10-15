using DataAccess.Scaffold.Attributes;
using DataAccess.Scaffold.ViewModels;
using Needletail.SampleModel.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needletail.SampleModel.Model.ViewModel
{
    [NeedletailViewModel]
    public class UserViewModel : ViewModelAutoLoadAndSave
    {

        [HasOne("UserAddress","AddressId","Address","Id")]
        [SelectFrom("DepartmentList","DepartmentId", "Department", "Id", "DepartmentName")]
        [HasMany("EquipmentList","Id","UserEquipment","UserId")]
        [HasManyNtoN("ProjectList","Id","UserProject","UserId","ProjectId","Project","Id")]
        public User User { get; set; }

        public Address UserAddress { get; set; }

        public IList<Department> DepartmentList { get; set; }

        public IList<Project> ProjectList { get; set; }

        public IList<UserEquipment> EquipmentList { get; set; }

    }
}
