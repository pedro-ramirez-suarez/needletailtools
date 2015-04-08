def view_index_template model
    name = model['name']

return <<template
@{
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";
    
}

<h2>#{name}</h2>
<table class="table table-striped" >
    <thead>
        <tr>
            #{get_columns_names(model)}
        </tr>
    </thead>
    <tbody data-bind="foreach: #{name}s">
        <tr>
            #{get_labels_index model}
            <td><a data-bind="attr: {href: '/#{name}/Details/' + Id}">Details</a></td>
            <td><a data-bind="attr: {href: '/#{name}/Edit/' + Id}">Edit</a></td>
            <td><a data-bind="click: $parent.remove">Remove</a></td>
        </tr>
    </tbody>
</table>

<a href="/#{name}/Create" class="btn btn-primary">Add</a>
    
<script>
    require(["/Scripts/app/#{name}.binding.js", 'underscore', 'moment'], function (modelBinding, _, moment) {
        var model = JSON.parse('@Html.Raw(ViewBag.#{name}s)');
            
        _.each(model, function(item) {
            #{format_properties(model, 'index')}
            modelBinding.add(item);
        });
    });
</script>
template
end

def get_labels_index model
    labels = ""
    elements = model.at_xpath("fields").elements


    elements.each do |node|    
        next if (node.name.to_s == "Id" || node.has_attribute?("SelectFrom") || node.has_attribute?("HasOne"))

        property_name = node.name
        labels += "
            <td>
                <label class='display-label' data-bind='text: #{property_name}'></label>
            </td>"
    end
    labels
end

def get_columns_names model
    labels = ""
    elements = model.at_xpath("fields").elements


    elements.each do |node|    
        next if (node.name.to_s == "Id" || node.has_attribute?("SelectFrom") || node.has_attribute?("HasOne"))

        property_name = node.name
        labels += "
            <th>
                #{property_name}
            </th>"
    end
    labels
end