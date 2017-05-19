jQuery(function () {
    
    jQuery('#Evento_Inicio').datetimepicker({
        format: 'd/m/y H:i',
        //onShow: function (ct) {
        //    this.setOptions({
        //        maxDate: jQuery('#Evento_Final').val() ? jQuery('#Evento_Final').val() : false,
        //        maxTime: jQuery('#Evento_Final').val() ? jQuery('#Evento_Final').val().substr(11,5) : false
        //    });
        //},
        timepicker: true,
        weeks: false,
        defaultSelect: true,
        closeOnDateSelect: true
    });

    jQuery('#Evento_Final').datetimepicker({
        format: 'd/m/y H:i',
        //onShow: function (ct) {
        //    this.setOptions({
        //        minDate: jQuery('#Evento_Inicio').val() ? jQuery('#Evento_Inicio').val() : false,
        //        minTime: jQuery('#Evento_Inicio').val() ? jQuery('#Evento_Inicio').val().substr(11,5) : false
        //    })
        //},
        timepicker: true,
        weeks: false,
        defaultSelect: true,
        closeOnDateSelect: true
    });

    jQuery('#FechaLimite').datetimepicker({
        format: 'd/m/y',
        //onShow: function (ct) {
        //    this.setOptions({
        //        minDate: new Date()
        //    })
        //},
        timepicker: false,
        weeks: false,
        defaultSelect: true,
        closeOnDateSelect: true
    });
});