def js_require_config_template
return <<template
var require = {
    baseUrl: '/Scripts',
    paths: {
        jquery: 'jquery-1.9.1',
        bootstrap: 'bootstrap',
        knockout: 'knockout-3.2.0',
        underscore: 'underscore',
        bootstrapValidator: 'bootstrapValidator',
        utils: 'Utils',
        moment: 'moment',
        bootstrapDateTimePicker: 'bootstrap-datetimepicker'
    }
};
template
end