def model_template model
fields = get_fields_for_model model.at_xpath("fields").elements

return <<template
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Needletail.DataAccess.Attributes;

namespace #{@solution_name_sans_extension}.Models
{
  public class #{model["name"]}
  {
      #{fields}
  }
}
template
end

def get_fields_for_model elements
    fields = ""

    elements.each do |node|    
        params = ""
        if node.has_attribute?("hasOne")
            if node.at_xpath("HasOne").attribute("ReferencedField").to_s == "Id"
               type = "int" 
            end
        elsif node.has_attribute?("hasMany")
            type = "List<int>"
        elsif node.name == "Id"
            type = "int"
        else
            type = "string"
        end

        attrParams = node.at_css("Validator") ? node.at_css("Validator").attribute_nodes : []
        
        if attrParams.length > 0
            attrParams.each do |attrb|
                params += "#{attrb.to_s}, "
            end
            params = "(#{params.chomp(', ')})"
        end

        attributes = node.has_attribute?('validator')? 
                    "[#{node.attribute('validator')}#{params}]\n" : ""

        #Remove attributes not supported
        attributes = ""

        if node.name == "Id"
            attributes += "[TableKey(CanInsertKey = false)]\n"
        end

        fields += "#{attributes}public #{type} #{node.name} { get; set; }\n\t\t"
    end
    fields
end