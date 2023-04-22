$(document).ready(() => {

    let cropper;

    let originalBackgroundSrc = document.getElementById("bg-pic-display").src;
    let modalImage = $("#modal-image");
    let modal = new bootstrap.Modal(document.getElementById('crop-pp-modal'), {
        backdrop: 'static'
    });
    let modalElement = $('#crop-pp-modal');

    $("#profile-pic-input").change(e => {
        let files = e.target.files

        if (files[0]) {
            let url = URL.createObjectURL(files[0]);

            modalImage.attr("src", url);
            modal.show();
        }

    });

    modalElement.on('shown.bs.modal',() => {

        let imageForCropping = document.getElementById('modal-image');
        cropper = new Cropper(imageForCropping, {
            aspectRatio: 1,
            viewMode: 3
        });

    });

    modalElement.on('hidden.bs.modal', () => {
        cropper.destroy();
    });


    $('#crop-pp').click(() => {

        let canvas = cropper.getCroppedCanvas({
            width: 480,
            height: 480,
        });

        $('#profile-pic-display').attr('src', canvas.toDataURL());

        canvas.toBlob(blob => {

            let reader = new FileReader();
            reader.readAsDataURL(blob);
            reader.onloadend = () => {
                document.getElementById("profile-pic-b64").value = reader.result;
            };

        }, 'image/jpeg');

        modal.hide();

    });


    $('#BackgroundPhotoInput').change(e => {

        let files = e.target.files;

        if (files[0]) {

            let reader = new FileReader();
            reader.readAsDataURL(files[0]);
            reader.onloadend = () => {
                document.getElementById("bg-pic-display").src = reader.result;
            };
        } else {
            document.getElementById("bg-pic-display").src = originalBackgroundSrc;
        }

    });

});