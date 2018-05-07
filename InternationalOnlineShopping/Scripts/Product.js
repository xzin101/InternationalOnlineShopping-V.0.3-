function ShowImagePreview(imageUploader, previewImage) {


    if (imageUploader.files.length > 0) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function SaveProduct(formData) {
    debugger;
  //  $.validator.unobtrusive.parse(formData);
    //if ($(formData).valid()) {

    //    var ajaxConfig = {

    //        type: 'POST',
    //        url: "/Product/AddOrEdit",
    //        data: new FormData(form),
    //        success: function (response) {
    //            $('#viewProductsId').html(response);
    //        }

    //    }

    //    if ($(form).attr('enctype') === "mutipart/form-data") {
    //        ajaxConfig["contentType"] = false;
    //        ajaxConfig["processData"] = false;

    //    }
    //    $.ajax(ajaxConfig);



    //}

    var ajaxConfig = {

        type: 'POST',
        url: "/Product/AddOrEdit",
        data: new FormData(formData),
        success: function (response) {
            $('#viewProductsId').html(response);
        }

    }
    if ($(formData).attr('enctype') == "mutipart/form-data") {
           ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;

        }
    $.ajax(ajaxConfig);
    return false;

}