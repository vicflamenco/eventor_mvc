$(document).ready(function () {
    $(".btnReenviar").click(function () {
        
        var participanteid = $(this).data('participanteid');
        var eventoid = $(this).data('eventoid');
        var nombre = $(this).data('nombre');
        var correo = $(this).data('correo');
        var url = $(this).data('url');

        $('#ImagenCargando_' + participanteid).show();

        $.post(url, { participanteId: participanteid, eventoId: eventoid }, function (data) {
            
            if (data)
                $('#txtContenido').html('Se ha enviado una invitación a <strong>' + nombre + " (" + correo + ")</strong>");
            else
                $('#txtContenido').html('No se pudo enviar una invitación a <strong>' + nombre + " (" + correo + ")</strong>.Revise que la dirección de correo electrónico sea válida.");

            $('#ImagenCargando_' + participanteid).hide();
            $('#modalReenviar').modal('show');
            
        });

    });

});