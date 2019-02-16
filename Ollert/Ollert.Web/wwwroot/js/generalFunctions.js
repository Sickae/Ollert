// ContentEditable div-ekből törli a böngésző által beszúrt <br> tageket ha nincs szöveg
$(document).on('keyup blur', 'div[contenteditable=true]', function () {
    if ($(this).text().length === 0) {
        $(this).children('br').remove();
    }
});