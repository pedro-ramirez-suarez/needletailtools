def controller_template model, keytype
    name = model['name']
    name_downcase = name.downcase
    viewModelSufix = ""    
    useRepositories = true

    if model.xpath("//entity").length > 1
        viewModelSufix = "ViewModel"    
        useRepositories = false
    end
    
return <<template
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using #{@solution_name_sans_extension}.Repositories;
using Needletail.DataAccess;
using System.Configuration;
using System.Web.Script.Serialization;
using Needletail.SampleModel.Model.Entity;
using Needletail.SampleModel.Model.ViewModel;


namespace #{@solution_name_sans_extension}.Controllers
{
    public class #{name}Controller : Controller
    {
        
        #{get_repositories(model, 'declaration')}

        public #{name}Controller() {
            #{get_repositories(model, 'init')}
        }

        //
        // GET: /#{name}/

        public ActionResult Index()
        {
            var #{name_downcase}s = #{name_downcase}Repository.GetAll();
            ViewBag.#{name}s = new JavaScriptSerializer().Serialize(#{name_downcase}s);
            
            return View();
        }

        //
        // GET: /#{name}/Details/id

        public ActionResult Details(#{keytype} id)
        {
            #{(get_repositories(model, 'use') if useRepositories)}
            #{init_view_model(model)}
            
            return View();
        }

        //
        // GET: /#{name}/Create

        public ActionResult Create()
        {
            #{(useRepositories ? 
            "var #{name} = new #{name}();
            ViewBag.#{name} = new JavaScriptSerializer().Serialize(#{name});" : 
            "var viewModel = new #{name}ViewModel();
            viewModel.FillData(primaryKey: new Guid());;
            ViewBag.#{name} = new JavaScriptSerializer().Serialize(viewModel);")}
            return View();
        }

        //
        // POST: /#{name}/Create

        [HttpPost]
        public ActionResult Create(#{name}#{viewModelSufix} model)
        {

            #{(useRepositories ? "model.Id = Guid.NewGuid();" +
            "#{name_downcase}Repository.Insert(model);" : 
            "model.#{name}.Id = Guid.NewGuid();
            #{get_has_ones(model)}
             model.Save();")}

            return Json(new { result = "Redirect", url = Url.Action("Index", "#{name}") });
        }

        //
        // GET: /#{name}/Edit/id

        public ActionResult Edit(#{keytype} id)
        {
            #{(useRepositories ? "var #{name_downcase} = #{name_downcase}Repository.GetSingle(where: new { Id = id });
                ViewBag.#{name} = new JavaScriptSerializer().Serialize(#{name_downcase});" : "var viewModel = new #{name}ViewModel();
                viewModel.FillData(primaryKey: id);
                ViewBag.#{name} = new JavaScriptSerializer().Serialize(viewModel);")}
            return View();
        }

        //
        // POST: /#{name}/Edit/id

        [HttpPost]
        public ActionResult Edit(#{name}#{viewModelSufix} model)
        {
            #{(useRepositories ? 
            "var result = #{name_downcase}Repository.UpdateWithWhere(values: model,
                            where: new {Id = model.Id});" : 
            "model.Save();")}

            return Json(new { result = "Redirect", url = Url.Action("Index", "#{name}") });
        }


        //
        // POST: /#{name}/Delete/5

        [HttpPost, ActionName("Delete")]
        public bool Delete(#{keytype} id)
        {

            var result = #{name_downcase}Repository.Delete(where: new {Id = id});
            return result;
        }
    }
}
template
end

def get_repositories model, action
    repositories = ""

    entity_name = model['name']
    if action == "declaration"
        repositories += "
        #{entity_name}Repository #{entity_name.downcase}Repository;"    
    elsif action == "init"
        repositories += "
        #{entity_name.downcase}Repository = new #{entity_name}Repository();"
    elsif action == "use"
            repositories += "
        var #{entity_name} = #{entity_name.downcase}Repository.GetSingle(where: new {Id = id});"        
    end 
        
    repositories
        
end

def init_view_model model
    name = model['name']
    view_bag = "ViewBag.#{name} = new JavaScriptSerializer().Serialize(#{name});"


    if model.xpath("//entity").length > 1
        view_bag = "
            var viewModel = new #{name}ViewModel();
            viewModel.FillData(primaryKey: id);
            ViewBag.#{name} = new JavaScriptSerializer().Serialize(viewModel);"
    end

    view_bag
end
 def get_has_ones model
    has_ones = ""
    model.xpath("//@HasOne").each do |node|
        main_entity_name = model['name']
        property_name = node
        id_name = node.parent.name

        nkg_obj = node.xpath("//entity")
        has_one = nkg_obj.search("entity[objectName=#{property_name}]")
        has_one_name = has_one.attribute('name')

        has_ones += "
            model.#{main_entity_name}.#{id_name} = Guid.NewGuid();
            model.#{property_name} = new #{has_one_name}(){Id = model.#{main_entity_name}.#{id_name}};"
    end

    has_ones
 end