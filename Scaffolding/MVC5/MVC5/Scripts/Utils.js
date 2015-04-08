define(['moment'], function (moment) {
    var formatDate = function (date) {  
        var result = moment(date);
        return result;
    };

    Utils = {        
      formatDate: formatDate
    };

    return Utils;
});