def format_properties model, file_type = ""
    result = ""
    property_name = ""
    entity_name = model['name']

    model.search("@validator").each do |node|
        case node.to_s
        when "date"
            property_name = node.parent.name

            case file_type
            when 'index'
                result += "item.#{property_name} = moment(item.#{property_name}).format('l');"
            when 'create_edit'
                result += "model.#{(entity_name + '.' if @is_view_model)}#{property_name} = moment(model.#{(entity_name + '.' if @is_view_model)}#{property_name});"
            else
                result += "model.#{(entity_name + '.' if @is_view_model)}#{property_name} = moment(model.#{(entity_name + '.' if @is_view_model)}#{property_name}).format('l');"
            end
        end 
    end    

    result
end

def get_selectfrom_template model, file_type
    result = ""
    select_name = ""
    property_name = ""
    list_name = ""
    display_field = ""
    entity_name = model['name']

    return result if model.search("SelectFrom").empty?

    model.search("//SelectFrom/entity").each do |node|
        select_name = node.attribute('name')
        list_name = node.attribute('listName')

        model.search("SelectFrom").each do |node|
            if node.parent.attribute('SelectFrom').to_s == list_name.to_s
                display_field = node.attribute('DisplayField')
                property_name = node.parent.name
            end
        end
    end    


    case file_type
    when 'binding_init'
        result = ",
        selected#{select_name}: ko.observable()"
    when 'binding_search'
    result = "that.selected#{select_name} = {};
            _.each(element.#{list_name}, function (item) {
                if (item.Id === element.#{entity_name}.#{property_name}) {
                    that.selected#{select_name} = item;
                }
            });"
    when 'validate_init'
        result = "viewModel.#{entity_name}s()[0].#{entity_name}.#{property_name} = viewModel.selected#{select_name} ? viewModel.selected#{select_name}.Id : '';"
    when 'edit_create'
        result = @form_fields[:select_from] %[select_name, property_name, list_name, display_field, select_name]
    end


    result
end

def get_datepicker_template model, file_type
    result = ""
    select_name = ""
    property_name = ""
    list_name = ""
    display_field = ""
    entity_name = model['name']

     return result unless model.search("//@validator='date'")

    model.search("@validator").each do |node|
        case node.to_s
        when "date"
            property_name = node.parent.name
            result += "model.#{(entity_name + '.' if @is_view_model)}#{property_name} = utils.formatDate(model.#{(entity_name + '.' if @is_view_model)}#{property_name});"
        end 
    end       


    case file_type
    when 'validate_init'
        result = "$('##{property_name}').datetimepicker({
            pickTime: false,
            useMinutes: false,               
            useSeconds: false,               
            useCurrent: false
        });"
    when 'validate_change'
        result = "$('##{property_name}')
        .on('dp.change dp.show', function (e) {
            // Revalidate the date when user change it
            $('.user_form').bootstrapValidator('revalidateField', '#{property_name}');
        });"
    
    when 'validate_use'
        result = "viewModel.#{entity_name}s()[0].#{entity_name}.#{property_name} = viewModel.dateSelected();"
    when 'edit_create'
        result = @form_fields[:datepicker] %[property_name, property_name, property_name]
    when 'binding_add'
        result = "var date = element.#{entity_name} ? element.#{entity_name}.#{property_name} : element.#{property_name};
            that.dateSelected = ko.observable(date);"
    end


    result
end
