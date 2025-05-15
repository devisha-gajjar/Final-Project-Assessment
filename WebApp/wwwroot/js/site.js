// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    /* Password Show & Hide Logic */
    $('.password-toggle-icon i').click(function () {
        let inputField = $(this).parent().prev('input');
        let isPassword = inputField.attr('type') === 'password';
        inputField.attr('type', isPassword ? 'text' : 'password');
        $(this).toggleClass('fa-eye fa-eye-slash');
    });

});
