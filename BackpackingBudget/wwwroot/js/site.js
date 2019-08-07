// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function onCreate() {
    var obj = document.getElementById("dockSidebar").ej2_instances[0];
    obj.mediaQuery = window.matchMedia('(min-width: 600px)');
}

document.addEventListener('DOMContentLoaded', function () {
    dockBar = document.getElementById('dockSidebar').ej2_instances[0];
    document.getElementById('toggle').onclick = function () {
        dockBar.toggle();
    };
});

$(function () {
    $('#btnSave').click(function () {
        $('myModal').modal('hide');
    });
});

$('#myModal').on('shown.bs.modal', function () {
    $('#myInput').trigger('focus')
})

function onChange(args) {
    this.label = 'CheckBox: ' + args.checked;
}


//<script>
//    $(document).ready(function(){
//        $("#some-form").validate({

//            rules: {
//                TextBox1: {
//                    required: true,
//                    number: true,
//                    min: 0,
//                    max: 100,
//                },
//                TextBox2: {
//                    required: true,
//                    number: true,
//                    min: 0,
//                    max: 100,
//                },
//            TextBox3: {
//                    required: true,
//                    number: true,
//                    min: 0,
//                    max: 100,
//                },
//            TextBox4: {
//                    required: true,
//                    number: true,
//                    min: 0,
//                    max: 100,
//                }

//            },
//            submitHandler: function (form) {
//                var total = parseInt($("#TextBox1").val()) + parseInt($("#TextBox2").val()) + parseInt($("#TextBox3").val()) + parseInt($("#TextBox4").val());
//                if (total != 100) {
//                    $(".error").html("Your total must sum to 100.")
//                    return false;
//                } else form.submit();
//            }

//        });
//    });
    
//</script>