def tests_controller_template model
	name = model['name']
    name_downcase = name.downcase

return <<template
using #{@solution_name_sans_extension}.Controllers;
using NSpec;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace #{@solution_name_sans_extension}.Tests
{
    class #{name}Controller_spec: nspec
    {
        #{name}Controller controller;
        void before_each()
        {   
            //Add any initialization code here
            //System.Diagnostics.Debugger.Launch();
            controller = new #{name}Controller(); 
        }
            
        void specify_controllerIndex()
        { 
            var result = controller.Index() as ViewResult;
            string #{name}List = result.ViewBag.#{name}s as string;
            #{name}List.should_not_be_empty(); 
        }

        void specify_faillMe()
        {
            "foo".should_not_be("fo1o");
        }
    }
}
template
end