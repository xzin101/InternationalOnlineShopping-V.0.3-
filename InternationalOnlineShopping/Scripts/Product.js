function ShowImagePreview(imageUploader, previewImage) {
    

    if (imageUploader.files.length>0) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function SaveProduct(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {

        var ajaxConfig = {

            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                $('#viewProductsId').html(response);
            }

        }

        if ($(form).attr('enctype') === "mutipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;

        }
        $.ajax(ajaxConfig);
           

        
    }
    return false;

}