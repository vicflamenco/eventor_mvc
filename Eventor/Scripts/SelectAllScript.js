﻿$(document).ready(function () {
    $("#chkSelectAll").change(function () {
        var checkboxes = $(this).closest('form').find(':checkbox');
        if ($(this).is(':checked')) {
            checkboxes.prop('checked', true);
        } else {
            checkboxes.prop('checked', false);
        }
    });
});
