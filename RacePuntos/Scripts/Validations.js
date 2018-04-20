jQuery(function ($) {
    emailRegExp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    $("#contrasena").focusout(function () {
        Validar_Password($("#contrasena").val());
    });

    //$("#correoElectronico").focusout(function () {
    //    if (!emailRegExp.test($(this).val())) {
    //        swal("Error", "Correo electronico incorrecto.", "error");
    //    }
    //});

    $("#submit_login").click(function () {
        if ($("#documento").val() != "" && $("#contrasena").val() != "") {
            $("#Login_Form").submit();
        } else {
            swal("Error", "Se debe diligenciar todos los campos.", "error");
        }
    });

    $("#rol").change(function () {
        switch ($(this).val()) {
            case "ADMIN":
                $("#cargo").val("1");
                break;
            case "USUARIO":
                $("#cargo").val("2");
                break;
            case "N/A":
                $("#cargo").val("3");
                break;
        }
    });

    $("#guarda_usr").click(function () {
        var Error = 0;
        var valid = "#Create_User input, #Create_User select";
        Error = ValidateForm(valid);
        if (Error > 0) {
            swal("Error", "Todos los campos deben estar diligenciados.", "error");
            return false;
        }

        $("#Create_User").submit();
    });

    $("#guarda_veh").click(function () {
        //var Error = 0;
        //var valid = "#Create_Vehi input, #Create_Vehi select";
        //Error = ValidateForm(valid);
        if (
            $("#documento_usuario").val() == "" ||
            $("#placa_vehiculo_cliente").val() == "" ||
            $("#marca_vehiculo").val() == "" ||
            $("#referencia_vehiculo").val() == "" ||
            $("#cilindraje_vehiculo").val() == ""
            ) {
            swal("Error", "Todos los campos deben estar diligenciados.", "error");
            return false;
        }

        $("#Create_Vehi").submit();
    });

    if ($(".selectpicker").length > 0) {
        $('.selectpicker').selectpicker({
            style: 'btn-default',
            size: 2
        });
    }

    if ($("#LisAquisicion").length > 0 || $("#LisUsuarios").length > 0) {
        $('#LisAquisicion, #LisUsuarios').DataTable({
            "language": {
                "sPaginationType": "full_numbers",
                "sProcessing": "",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "NingÃºn dato disponible en esta tabla",
                "sInfo": "Registros _START_ al _END_ de _TOTAL_",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Ãšltimo",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
            },
            "bLengthChange": false, //thought this line could hide the LengthMenu
            "bInfo": false,
            lengthMenu: [[5], [5]],
            destroy: true,
            //"sDom": 'T<"clear">lfrtip',
            //"tableTools": {
            //    "sSwfPath": '/Scripts/plugins/dataTables/copy_csv_xls_pdf.swf',
            //    "aButtons": [
            //      {
            //          "sExtends": "collection",
            //          "sButtonText": "exportar",
            //          "aButtons": [
            //            {
            //                "sExtends": "copy",
            //                "sButtonText": "Copiar al portapapeles",
            //                "bHeader": true
            //            },
            //            {
            //                "sExtends": "xls",
            //                "bHeader": true,
            //                "sTitle": "Reporte Ticket Empresa"
            //            },
            //            {
            //                "sExtends": "pdf",
            //                "sPdfOrientation": "landscape",
            //                "sTitle": "Reporte"
            //            }
            //          ]
            //      }
            //    ]
            //}
        });
    }

});

function validaNum(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros
    patron = /[0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function Validar_Password(clave) {
    var expreg = /(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*/;

    if (!expreg.test(clave) && $("#contrasena").val().trim() != "") {
        alert("La clave ingresa no es valida, verifique.");
        $("#contrasena").val("");
        return false;
    }
}

function ValidateForm(CampValidate) {
    var Error = 0;
    $("" + CampValidate + "").each(function () {        
        if ($(this).val() == "") {
            console.log("id: " + $(this).attr("id") + " -> value: " + $(this).val());
            Error++;
        }
    });

    return Error;
}