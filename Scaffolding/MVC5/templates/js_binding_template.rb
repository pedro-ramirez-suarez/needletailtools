def js_binding_template model
    name = model['name']
return <<template
define(['jquery', 'knockout', 'underscore', 'moment'], function ($, ko, _, moment) {
    var model#{name} = [
    ];

    ko.bindingHandlers.datetimepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {

            var options = {
                pickTime: false,
                defaultDate: AppViewModel.dateSelected()
            };

            $(element).parent().datetimepicker(options);

            ko.utils.registerEventHandler($(element).parent(), "change.dp", function (event) {
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    var thedate = $(element).parent().data("DateTimePicker").getDate();
                    value(moment(thedate).toDate());
                }
            });
        },
        update: function (element, valueAccessor) {
            var widget = $(element).parent().data("DateTimePicker");
            //when the view model is updated, update the widget
            var thedate = new Date(parseInt(ko.utils.unwrapObservable(valueAccessor()).toString().substr(6)));
            widget.setDate(thedate);
        }
    };

    var AppViewModel = {
        #{name}s: ko.observableArray(
            model#{name}
        ),

        add : function (element) {
            var that = this;

            #{get_selectfrom_template(model, 'binding_search')}
            #{get_datepicker_template(model, 'binding_add')}
            that.#{name}s.push(element);
        },

        remove: function () {
            var self = this;
            $.post("/#{name}/Delete", { id: this.Id }, function (success) {
                if (Boolean(success)) {
                    AppViewModel.#{name}s.remove(self);
                }
            });
        },

        dateSelected: ko.observable()#{get_selectfrom_template(model, 'binding_init')}


    };

    $(document).ready(function () {
        ko.applyBindings(AppViewModel);
    });
    return AppViewModel;

});
template
end