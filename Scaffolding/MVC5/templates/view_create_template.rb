def view_create_template model
fields = get_fields model
entity_name = model['name']

return <<template
@{
    ViewBag.Title = "Create";
    Layout = "~/Views/Shared/_Layout.cshtml";
}


<div class="page-header text-center">
    <h2>Create</h2>
</div>
<form id="create_#{entity_name.downcase}_form" method="post" class="form-horizontal #{model['name'].downcase}_form" action="create" role="form" data-bind="foreach: #{entity_name}s">
    #{fields}
    <div class="form-group">
        <div class="col-sm-offset-2 col-sm-10">
            <!-- Do NOT use name="submit" or id="submit" for the Submit button -->
            <button type="submit" class="btn btn-primary">Create</button>

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

def get_fields model
    elements = model.at_xpath("fields").elements
    fields = ""
    entity_name = ""

    if elements.xpath("//entity").length > 1
        entity_name = elements.xpath("//entity").first['name'] + "."
    end

    elements.each do |node|    
        property_name = node.name
        next if property_name.to_s == "Id"

        if node.at_css("SelectFrom")
            fields += get_selectfrom_template model, 'edit_create'
        elsif node.at_css("HasOne")
            next
        elsif node.attribute('validator').to_s == "date"
            fields += get_datepicker_template(model, 'edit_create')
        else
            fields += @form_fields[:text] %[property_name, property_name, "data-bind='value: #{entity_name}#{property_name}'" ]
        end
        
    end
    fields

end
