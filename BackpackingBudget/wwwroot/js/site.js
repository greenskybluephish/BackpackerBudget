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
