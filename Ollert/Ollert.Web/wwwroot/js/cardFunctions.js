/* ############
   ## Events ##
   ############
*/

// Új kártya hozzáadásnál Enter és Esc gombok működése
$(document).on('keydown', '.empty-card-input .cardlist-card-name-input', function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        addNewCard(this);
    } else if (event.keyCode === 27) {
        toggleNewCardWindow(this);
    }
});

// Kártya szerkesztésnél Enter és Esc gombok működése
$(document).on('keydown', '.edit-card-input .cardlist-card-name-input', function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        var card = $(this).closest('.cardlist-card');
        var cardId = card.data('id');
        var cardName = card.find('.cardlist-card-name-input').text();

        renameCard(cardId, cardName);
    } else if (event.keyCode === 27) {
        toggleEditCardWindow(this);
    }
});

// Kártya szerkesztésnél mentés gomb aktiválása/deaktiválása
$(document).on('keyup', '.edit-card-input .cardlist-card-name-input', function () {
    var name = $(this).text();
    $(this).closest('.edit-card-input').find('.btn-save').toggleClass('disabled', name.length === 0 || name.length > 255);
});

// Kártya szerkesztés mentése
$(document).on('click', '.edit-card-input .btn-save', function () {
    var card = $(this).closest('.cardlist-card');
    var cardId = card.data('id');
    var cardName = card.find('.cardlist-card-name-input').text();

    renameCard(cardId, cardName);
});

// Kártya törlése
$(document).on('click', '.cardlist-icon-delete', function () {
    deleteCard(this);
});

/* ###############
   ## Functions ##
   ###############
*/

// Új kártya hozzáadása ablak ki/be kapcsolása
function toggleNewCardWindow(caller) {
    var activeWindow = $('.cardlist-list').find('.empty-card-input');

    if (activeWindow.length > 0) {
        var container = activeWindow.closest('.cardlist-container');
        container.find('.cardlist-content.new').removeClass('new');
        container.find('.cardlist-card-new').slideDown('fast');
        activeWindow.slideUp('fast', () => activeWindow.remove());
    } else {
        var container = $(caller).closest('.cardlist-container');
        var inputWindow = $('.templates > .empty-card-input').clone().hide();
        container.find('.cardlist-card-new').slideUp('fast');
        container.find('.cardlist-content').addClass('new').append(inputWindow);
        inputWindow.slideDown('fast', () => inputWindow.find('.cardlist-card-name-input').focus());
    }
}

// Kártya szerkesztés ablak ki/be kapcsolása
function toggleEditCardWindow(caller) {
    var activeWindow = $(caller).closest('.edit-card-input');

    if (activeWindow.length > 0) {
        var card = activeWindow.closest('.cardlist-card');
        activeWindow.remove();

        card.removeClass('edit');
        card.find('.cardlist-card-content, .cardlist-card-icon').show();
    } else {
        var card = $(caller).closest('.cardlist-card');
        var cardId = card.data('id');
        var cardName = card.find('.cardlist-card-name').text();
        var cardInput = $('.templates > .edit-card-input').clone();
        var cardNameInput = cardInput.find('.cardlist-card-name-input');

        card.addClass('edit');
        card.children('div').hide();
        card.append(cardInput);
        cardInput.find('.cardlist-card').attr('data-id', cardId);
        cardNameInput.text(cardName).focus();
        setDivCaretToEnd(cardNameInput[0]);
    }
}

// Új kártya hozzáadása
function addNewCard(caller) {
    var emptyCard = $('.templates > .empty-card > .cardlist-card').clone();
    var listContainer = $(caller).closest('.cardlist-container');
    var cardName = $(caller).closest('.empty-card-input').find('.cardlist-card-name-input').text();
    var cardDesc = $('<input id="card_Description" type="hidden">');

    $.post('../../CardList/AddNewCard', {
        cardListId: listContainer.data('id'),
        cardName: cardName
    }).done(function (data) {
        if (data.success) {
            emptyCard.attr('data-id', data.id);
            emptyCard.find('.cardlist-card-name').text(cardName);
            emptyCard.append(cardDesc);
            listContainer.find('.cardlist-content').append(emptyCard);
            toggleNewCardWindow() 
            emptyCard.show();
        }
    });
}         

// Kártya átnevezés (nézet ablakból)
function renameCard(cardId, cardName) {
    $.post('../../Card/Rename', {
        id: cardId,
        cardName
    }).done(function (data) {
        if (data.success) {
            var card = $('.cardlist-card[data-id=' + cardId + ']');
            card.find('.cardlist-card-name').text(cardName);
            toggleEditCardWindow(card[1]);
        }
    });
}

// Kártya törlése
function deleteCard(caller) {
    var cardListId = $(caller).closest('.cardlist-container').data('id');
    var cardId = $(caller).closest('.cardlist-card').data('id');

    $.post('../../CardList/RemoveCard', {
        cardListId,
        cardId
    }).done(function (data) {
        if (data.success) {
            $('.cardlist-card[data-id=' + cardId + ']').remove();
        }
    });
}