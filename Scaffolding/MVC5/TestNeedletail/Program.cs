using Needletail.SampleModel.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNeedletail
{
    class Program
    {
        static void Main(string[] args)
        {

            var vm = new UserViewModel();
            
            //vm.User = new Needletail.SampleModel.Model.Entity.User();
            //vm.UserAddress = new Needletail.SampleModel.Model.Entity.Address();
            //vm.UserAddress.Id = Guid.NewGuid();
            //vm.User.AddressId = vm.UserAddress.Id;
            //vm.User.Id = Guid.NewGuid();

            //vm.User.Name = "Test";
            //vm.User.SubscriptionDate = DateTime.Now;
            //vm.UserAddress.Street = "Sample";
            //vm.UserAddress.Phone = "45465454656";
            //vm.UserAddress.ZipCode = "58000";
            //vm.Save();
            vm.FillData(primaryKey: Guid.Parse("459E6924-E67C-4DC8-8471-3176D912694A")); //459E6924-E67C-4DC8-8471-3176D912694E

            //check if it works

              Console.ReadKey();

        }
    }
}
