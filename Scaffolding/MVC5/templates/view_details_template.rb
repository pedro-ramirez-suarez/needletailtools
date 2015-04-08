def view_details_template model
    labels = get_labels model, model.at_xpath("fields").elements
    labels += get_has_many_labels model
    entity_name = model['name']

return <<template
@{
    ViewBag.Title = "Details";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Details</h2>
<div data-bind="foreach: #{model['name']}s">
    <table class="table table">
        <thead>
            <tr>
                <th>
                    #{model['name']}
                </th>
            </tr>
        </thead>
        <tbody>
            #{labels}
        </tbody>
    </table>

    <a data-bind="attr: {href:'/#{model['name']}'}" class="btn btn-primary">Back</a>
    <a data-bind="attr: {href:'/#{model['name']}/Edit/' + #{(entity_name + '.' if @is_view_model)}Id}" class="btn btn-primary">Edit</a>
</div>

<script>
    require(["/Scripts/app/#{model['name']}.binding.js", "moment"], function (modelBinding, moment) {
        var model = JSON.parse('@Html.Raw(ViewBag.#{entity_name})');
        #{format_properties(model)}        
        modelBinding.add(model);
    });
</script>

template
end

def get_labels entity, elements
    labels = ""

    if entity.xpath("//entity").length > 1
        if entity.attribute("objectName")
            entity_name = "#{entity.attribute('objectName')}."
        else
            entity_name = "#{entity.attribute('name')}."
        end
    end

    elements.each do |node|    
        next if node.name.to_s == "Id"

        property_name = node.name

        if node.at_css("HasOne")
            id = node.attribute('HasOne').to_s
            nkg_obj = node.xpath("//entity")
            new_entity = nkg_obj.search("entity[objectName=#{id}]")

            labels += get_labels new_entity, new_entity.at_xpath("fields").elements

        else
            if node.at_css("SelectFrom")
                next
                #id = node.attribute('SelectFrom').to_s
                #nkg_obj = node.xpath("//entity")
                #new_entity = nkg_obj.search("entity[listName=#{id}]")
                #entity_name = "#{new_entity.attribute('listName')}[0]."
                #property_name ="#{node.at_css("SelectFrom").attribute('DisplayField')}"
            end

            labels += "
            <tr>
                <td>#{property_name}:</td>
                <td>
                    <label class='display-label' data-bind='text: #{entity_name}#{property_name}'></label>
                </td>
            </tr>"
        end
        
    end
    labels
end

def get_has_many_labels model
    labels = ""
    has_many = model.xpath("//HasMany")
    has_many.each do |node|
        entityName = node.at_xpath("entity").attribute("name")
        list_name = node.at_xpath("entity").attribute("listName")
            labels += "
            <tr>
                <td>#{entityName}(s):</td>
                <td>
                    <ul data-bind='foreach: #{list_name}'>
                        <li>
                            <a data-bind='text: Name, attr:{href:\"/#{entityName}/edit/\" + Id}'></a>       
                        </li>
                    </ul>
                </td>
            </tr>"
    end
    labels
end