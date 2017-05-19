$(function () {
    $('.AddCart').click(function () {
        
        var ventaMax = $(this).data('ventamax');
        var ventaMin = $(this).data('ventamin');
        
        $('#Cantidad').find('option').remove();

        for(var i = ventaMin; i <= ventaMax; i++)
            $('#Cantidad').append($('<option>', { text: i, value: i }));
       
        var strPrecioUnitario = $(this).data('preciounitario');
        var floatPrecioUnitario = new Number(strPrecioUnitario.replace(/[^0-9\.]+/g, ""));
        var floatCantidad = parseFloat($('#Cantidad').val());
        var subtotal = floatPrecioUnitario * floatCantidad;
            
        $('#PrecioUnitario').text(strPrecioUnitario);
        $('#Subtotal').text('$' + subtotal);

        $('#btnAgregarAlCarrito').data('entradaid', $(this).data('entradaid'));
        $('#btnAgregarAlCarrito').data('precio', floatPrecioUnitario);
        $('#btnAgregarAlCarrito').data('url', $(this).data('url'));

        $('#myModal').modal("show");

    });

    $('#Cantidad').change(function () {
        var strPrecioUnitario = $('#PrecioUnitario').text();
        var floatPrecioUnitario = new Number(strPrecioUnitario.replace(/[^0-9\.]+/g, ""));
        var floatCantidad = parseFloat($('#Cantidad').val());
        var subtotal = floatPrecioUnitario * floatCantidad;

        $('#PrecioUnitario').text(strPrecioUnitario);
        $('#Subtotal').text('$' + subtotal);
    });

    $('#btnAgregarAlCarrito').click(function () {

        $('#ImgLoader').show();
        var url = $(this).data('url');

        $.post(
            url,
            {
                entradaId: $(this).data('entradaid'),
                cantidad: $('#Cantidad').val(),
                precio: $(this).data('precio')
            }).done(function (data) {
                if (!data)
                    alert('Error al agregar item. Intente nuevamente.');
                $('#myModal').modal("hide");
            }).fail(function (xhr, textStatus, errorThrown) {
                alert('Error al agregar item. Intente nuevamente. Respuesta: ' + xhr.responseText);
            }).always(function () {
                $('#ImgLoader').hide();
            });
    });
});
