def view_edit_template model
fields = get_fields_edit model
entity_name = model['name']
name_downcase = entity_name.downcase
return <<template
@{
    ViewBag.Title = "Edit";
    Layout = "~/Views/Shared/_Layout.cshtml";
}


<div class="page-header text-center">
    <h2>Edit</h2>
</div>
<form id="edit_#{name_downcase}_form" method="post" class="form-horizontal #{name_downcase}_form" action="Edit" role="form" data-bind="foreach: #{model['name']}s">
    #{fields}

    <div class="form-group">
        <div class="col-sm-offset-2 col-sm-10">
            <!-- Do NOT use name="submit" or id="submit" for the Submit button -->
            <button type="submit" class="btn btn-primary">Save</button>

        </div>
    </div>
</form>

<script>
    require(["/Scripts/app/#{model['name']}.binding.js", "/Scripts/app/#{model['name']}.validate.js", "moment"], function (modelBinding, modelValidate, moment) {
        var model = JSON.parse('@Html.Raw(ViewBag.#{model['name']})');
        #{format_properties(model, 'create_edit')}  
        modelBinding.add(model);
        modelValidate.initViewModel(modelBinding);
    });
</script>
template
end

def get_fields_edit model
    elements = model.at_xpath("fields").elements
    fields = ""
    entity_name = ""

    if elements.xpath("//entity").length > 1
        entity_name = elements.xpath("//entity").first['name'] + "."
    end

    elements.each do |node|    
        property_name = node.name

        if property_name.to_s == "Id"
            fields += @form_fields[:hidden] %[property_name, "data-bind='value: #{entity_name}#{property_name}'"]
        elsif node.at_css("SelectFrom")
            fields += get_selectfrom_template model, 'edit_create'
        elsif node.at_css("HasOne")
            reference = node.attribute('HasOne')
            nkg_obj = node.xpath("//entity")
            has_one = nkg_obj.search("entity[objectName=#{reference}]")
            has_one_name = has_one.attribute('name')

            fields += @form_fields[:button] %[has_one_name, "data-bind=\"attr:{href: '/#{has_one_name}/edit/' + #{entity_name}#{property_name}}\"", property_name]
        elsif node.attribute('validator').to_s == "date"
            fields += get_datepicker_template(model, 'edit_create')
        else
            fields += @form_fields[:text] %[property_name, property_name, "data-bind='value: #{entity_name}#{property_name}'"]
        end
        
    end
    fields

end
