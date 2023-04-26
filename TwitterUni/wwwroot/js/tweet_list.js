$(document).ready(() => {

    $(".main-tweet").click(e => {

        let id = e.target.closest('.main-tweet').id.split(',')[1];
        let path = window.location.pathname.replaceAll ('/', '-');

        window.location.href = `/Tweet/One/${id}/${path}`;
    });

});