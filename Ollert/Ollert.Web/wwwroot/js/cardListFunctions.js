/* ############
   ## Events ##
   ############
*/

// Új lista hozzáadásnál Enter és Esc gombok működése
$(document).on('keyup', '.cardlist-new-input', function (event) {
    if (event.keyCode === 13) { // Enter
        addNewCardList();
    } else if (event.keyCode === 27) { // Escape
        toggleNewCardListWindow();
    }
});

// Lista cím input kikattintásánál elmenti a módosításokat
$(document).on('blur', '.cardlist-title', function () {
    if ($(this).val().length > 0) {
        renameCardList(this);
    } else {
        $(this).val($(this).data('title'));
    }
    $(this).caretToStart();
});

// Lista cím szerkesztésénél Enter és Esc gombok működése
$(document).on('keyup', '.cardlist-title', function (event) {
    if (event.keyCode === 13) {
        $(this).blur();
    } else if (event.keyCode === 27) {

        $(this).val('');
        $(this).blur();
    }
});

/* ###############
   ## Functions ##
   ###############
*/

 // Új lista hozzáadása ablak ki/be kapcsolása
 function toggleNewCardListWindow() {
    $('input.cardlist-new-input').val('');
    $('.cardlist-new').toggle();
    var inputContainer = $('.cardlist-new-input-container').slideToggle('fast');

    if (inputContainer.css('display') === 'block') {
        $('.cardlist-new-input').focus();
    }
}

// Új lista hozzáadása
function addNewCardList() {
    var boardId = $('#Id').val();
    var emptyList = $('.templates > .empty-cardlist > .cardlist-container').clone();
    var inputContainer = $('.cardlist-new-input-container').closest('.cardlist-container');
    var title = inputContainer.find('input.cardlist-new-input').val();

    emptyList.find('.cardlist-title').val(title);
    inputContainer.find('.cardlist-new').show();
    inputContainer.find('.cardlist-new-input-container').hide();
    $('.cardlist-list').append(emptyList).append(inputContainer);

    $.post('../../Board/AddNewCardList', {
        boardId: boardId,
        cardListName: title
    }).done(function (data) {
        if (data.success) {
            var id = data.id;
            emptyList.attr('data-id', id);
            emptyList.find('.cardlist-title').attr('data-title', data.name);
        }
    });
}

// Lista átnevezése
function renameCardList(caller) {
    var listContainer = $(caller).closest('.cardlist-container');
    $.post('../../CardList/Rename', {
        id: listContainer.data('id'),
        cardListName: $(caller).val()
    }).done(function (data) {
        if (data.success) {
            $(caller).attr('data-title', data.name);
        } else {
            $(caller).val($(caller).data('title'));
        }
    });
}

// Lista törlése
function deleteCardList(caller) {
    var boardId = $('#Id').val();
    var list = $(caller).closest('.cardlist-container');
    var listId = list.data('id');

    $.post('../../Board/RemoveCardList', {
        boardId: boardId,
        cardListId: listId
    }).done(function (data) {
        if (data.success) {
            list.remove();
        }
    });
}

// Összes kártya törlése az adott listából
function clearAllCards(caller) {
    var list = $(caller).closest('.cardlist-container');
    var listId = list.data('id');

    $.post('../../CardList/RemoveAllCards', {
        id: listId
    }).done(function (data) {
        if (data.success) {
            list.find('.cardlist-card').remove();
        }
    });
}