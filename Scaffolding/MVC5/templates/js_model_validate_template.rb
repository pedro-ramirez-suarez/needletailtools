def js_model_validate_template model
fields = get_validators model.at_xpath("fields").elements
name  = model['name'].to_s
name_downcase  = name.downcase

return <<template
define(['jquery', 'bootstrapValidator', 'moment', 'bootstrapDateTimePicker'], function ($) {
    var initValidator = function () {
        #{get_datepicker_template model, 'validate_init'}

        $(".#{name_downcase}_form").bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                #{fields}
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();

            var $form = $(e.target);

            #{get_selectfrom_template(model, 'validate_init')}

            #{get_datepicker_template(model, 'validate_use')}

            var jsonObj = {
                model: viewModel.#{name}s()[0]
            };
            
            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                dataType: "json",
                data: JSON.stringify(jsonObj),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                  window.location = response.url;  
                }
            });
        });
    };

    var initViewModel = function (model) {
        viewModel = model;
    };

    #{get_datepicker_template model, 'validate_change'}
    
    $(document).ready(function () {
        initValidator();
    });

    return { initViewModel: initViewModel };
});
template
end

def get_validators elements
    fields = ""

    elements.each do |node|    
        params = ""
        js_validators = ""

        validators = node.has_attribute?("validator") ? node.attribute("validator").to_s.split(' ') : []

        if node.at_css("HasOne")
            id = node.attribute('HasOne').to_s
            nkg_obj = node.xpath("//entity")
            new_entity = nkg_obj.search("entity[objectName=#{id}]")
            fields += get_validators new_entity.at_xpath("fields").elements
        end
        
        attrParams = node.at_css("Validator") ? node.at_css("Validator").attribute_nodes : []
        
        if attrParams.length > 0
            attrParams.each_with_index do |attrb, index|
                attributeName = attrb.name.to_s.downcase
                attributeValue = attrb.to_s
                if attributeName == "regexp"
                    attributeValue = "/#{attributeValue}/"
                end
                params += "#{attributeName}: #{attributeValue}"

                if index < (attrParams.length - 1)
                    params += ",\n"
                end
            end
        end

        validators.each_with_index  do |validator, index|

            validator = (validator == 'required') ? 'notEmpty' : validator
            js_validators += "#{validator}: {
                            #{params}
                          }"
            params = ""
                          
            if index < (validators.length - 1)
                js_validators += ",\n"
            end
        end

        fields += "
                #{node.name}: {
                     message: 'The #{node.name} is not valid',
                     validators: {
                        #{js_validators}
                     }
                   },\n"
    end
    fields.chomp()
end