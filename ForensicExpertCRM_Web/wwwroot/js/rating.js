$('input[type="radio"]').click(function () {
    var theNumber = $(this).attr('id').slice(-1);
    $(this).siblings('label').each(function () {
        var sibNumber = $(this).attr('for').slice(-1);
        if (sibNumber <= theNumber) {
            $(this).addClass('on');
        } else {
            $(this).removeClass('on');
        }
    });
});