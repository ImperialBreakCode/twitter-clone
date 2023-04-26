$(document).ready(() => {

    $(".main-tweet").click(e => {

        let clickedElement = e.target.localName;
        let noActionElements = ['i', 'button', 'a'];

        if (!noActionElements.includes(clickedElement)) {

            let id = e.target.closest('.main-tweet').id.split(',')[1];
            let path = window.location.pathname.replaceAll('/', '-');

            window.location.href = `/Tweet/One/${id}/${path}`;

        }
    });

});