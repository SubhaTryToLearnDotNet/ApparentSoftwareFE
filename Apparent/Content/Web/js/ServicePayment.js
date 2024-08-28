$(document).ready(function () {
    ($("#loder01").css("display", "block"));
    $("#loder01").hide()
    $(document).ajaxStart(function () {
        ($("#loder01").css("display", "block"));
    });

    $(document).ajaxStop(function () {
        $("#loder01").hide()
    })


    $('#CardNumber').on('keyup', function (e) {
        var val = $(this).val();
        var newval = '';
        val = val.replace(/\s/g, '');
        for (var i = 0; i < val.length; i++) {
            if (i % 4 == 0 && i > 0) newval = newval.concat(' ');
            newval = newval.concat(val[i]);
        }
        $(this).val(newval);
    });


    $('#btnPayment').click(function () {
        if (!User_validate()) { return; }
        if (!Card_validate()) { return; }
        

        const fd = new FormData();
        fd.append('User_Name', document.getElementById('User_Name').value);
        fd.append('Email', document.getElementById('Email').value);
        fd.append('CardholderName', document.getElementById('CardholderName').value);
        fd.append('CardNumber', document.getElementById('CardNumber').value);
        fd.append('year', document.getElementById('year').value);
        fd.append('month', document.getElementById('month').value);
       /* fd.append('CVV', document.getElementById('CVV').value);*/
        fd.append('CVV', document.getElementById('CVV').value);
        fd.append('Amount', $('#Amount').val().trim());
        fd.append('Notes', document.getElementById('Notes').value);
        $.ajax({
            url: '/Service/card_payment_submit',
            method: 'POST',
            async: true,
            contentType: false,
            cache: false,
            processData: false,
            data: fd,
            success: function (data, textStatus, xhr) {
                if (data.code == 200) {
                    showSuccessUrl(data.msg,data.url);
                } else if (data.code == 400) {
                    ErrorMsg(data.msg);
                }
                else if (data.code == 500) {
                    ErrorMsg(data.msg);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                ErrorMsg('Something went wrong!');
            }
        });
    });

});

function Card_validate() {
    if (document.getElementById('CardholderName').value.length == 0) {
        showInfoMsg('Please fill card holder.');
        return false;
    }
    if (document.getElementById('CardNumber').value.length == 0) {
        showInfoMsg('Please fill card number.');
        return false;
    }
    if (document.getElementById('month').value.length == 0) {
        showInfoMsg('Please select month.');
        return false;
    }
    if (document.getElementById('year').value.length == 0) {
        showInfoMsg('Please select year.');
        return false;
    }
   
    if (document.getElementById('CVV').value.length == 0) {
        showInfoMsg('Please fill cvv.');
        return false;
    }
    
    return true;


}
function User_validate() {

    var amountValue = $('#Amount').val().trim();

    if (document.getElementById('User_Name').value.length == 0) {
        showInfoMsg('Please fill name.');
        return false;
    }
    if (document.getElementById('Email').value.length == 0) {
        showInfoMsg('Please fill email');
        return false;
    }
    if (!validateEmail(document.getElementById('Email').value)) {
        showInfoMsg('Please fill valid email');
        return false;
    }
    if (amountValue === '') {
        
        showInfoMsg('Amount is required.');
        return false;
    }

    if (isNaN(amountValue) || parseFloat(amountValue) <= 0) {
        showInfoMsg('Please enter a valid positive number for the Amount.');
       
        return false;
    }
   
    return true;


}