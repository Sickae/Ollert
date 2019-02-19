// Tábla átnevezése cím mező fókusz elvesztésekor
$(document).on('blur', '.nav-board-title', function () {
    var title = $(this).val();

    if (title.length === 0) {
        $(this).val($(this).data('name'));
    } else {
        var boardId = $('#Id').val();

        $.post('../../Board/Rename', {
            id: boardId,
            name: title
        }).done(function (data) {
            if (data.success) {
                $(this).attr('data-name', title);
            }
        });
    }
});

// Tábla cím mezőnél Enter és Esc gombok működése
$(document).on('keydown', '.nav-board-title', function (event) {
    if (event.keyCode === 13) {
        $(this).blur();
    } else if (event.keyCode === 27) {
        $(this).val('').blur();
    }
});