/* ############
   ## Events ##
   ############
*/

// Kártya nézet megjelenítése
$(document).on('click', '.cardlist-card-content', function () {
    showCardDetails(this);
});

// Kártya nézet elrejtése
$(document).on('click', '.dim-overlay', function () {
    hideCardDetails();
});

// Kártya nézet bezáró ikon
$(document).on('click', '.card-details-close', function () {
    hideCardDetails();
});

// Kártya nézetben címből kikattintva vissza az eredeti formázásra és a módosítások mentése ha kell
$(document).on('blur', '.card-details-title', function () {
    var cardId = $(this).closest('.card-details').data('id');
    var cardName = $(this).text();

    if (cardName.length > 0 && cardName.length <= 255) {
        renameCard(cardId, cardName);
    } else {
        var originalName = $('.cardlist-card[data-id=' + cardId + ']').find('.cardlist-card-name').text();
        $(this).text(originalName);
    }
});

// Kártya nézetben cím szerkesztésnél Enter és Esc gombok működése
$(document).on('keydown', '.card-details-title', function (event) {
    var cardId = $(this).closest('.card-details').data('id');

    if (event.keyCode === 13) {
        event.preventDefault();
        var title = $(this).text();
        if (title.length > 0 && title.length <= 255) {
            $(this).blur();
        }
    }
    else if (event.keyCode === 27) {
        var originalName = $('.cardlist-card[data-id=' + cardId + ']').find('.cardlist-card-name').text();
        $(this).text(originalName).blur();
    }
});

// Kártya nézetben leírás mező szerkesztése
$(document).on('blur', '.card-details-description-text', function () {
    var cardId = $(this).closest('.card-details').data('id');
    var cardDesc = $(this).text();

    $.post('../../Card/SetDescription', {
        id: cardId,
        description: cardDesc
    }).done(function (data) {
        if (data.success) {
            var card = $('.cardlist-card[data-id=' + cardId + ']');
            
            card.find('#card_Description').val(cardDesc);
            card.find('.cardlist-card-icon-description').toggle(cardDesc.length > 0);
        }
    });
});

// Kártya nézetben leírás mező szerkesztésnél Esc gomb működése
$(document).on('keydown', '.card-details-description-text', function (event) {
    if (event.keyCode === 13 && event.ctrlKey) {
        event.preventDefault();
        $(this).blur();
    }
    else if (event.keyCode === 27) {
        var cardId = $(this).closest('.card-details').data('id');
        var originalDesc = $('.cardlist-card[data-id=' + cardId + ']').find('#card_Description').val();
        $(this).text(originalDesc).blur();
    }
});

// Kártya nézetben új komment név megadás után kurzor előre mozgatása
$(document).on('blur', '.comment-new-author', function () {
    $(this).caretToStart();
});

// Kártya nézetben új komment mezőben Esc gomb működése
$(document).on('keydown', '.comment-new-text', function (event) {
    if (event.keyCode === 13 && event.ctrlKey) {
        event.preventDefault();
        $(this).siblings('.btn-save').click();
    }
    else if (event.keyCode === 27) {
        $(this).text('').blur();
    }
});

// Kártya nézetben új komment mentés gomb aktiválása/deaktiválása
$(document).on('keydown keyup', '.comment-new-text', function () {
    $(this).siblings('.btn-save').toggleClass('disabled', $(this).text().length === 0);
});

// Kártya nézetben új komment mentése
$(document).on('click', '.comment-new > .btn-save', function () {
    var comment = $(this).closest('.comment-new');
    var author = comment.find('.comment-new-author').val();
    var text = comment.find('.comment-new-text').text();
    var cardId = comment.closest('.card-details').data('id');

    if (text.length > 0) {
        $.post('../../Card/AddNewComment', {
            id: cardId,
            author,
            text
        }).done(function (data) {
            if (data.success) {
                var newComment = $('.templates > .comment').clone();
                var card = $('.cardlist-card[data-id=' + cardId + ']');

                newComment.attr('data-id', data.id);
                newComment.find('.comment-author').text(data.name);
                newComment.find('.comment-date').text(data.date);
                newComment.find('.comment-text').text(text);

                comment.find('.comment-new-text').text('').blur();
                comment.closest('.card-details-content').find('.card-details-comments').append(newComment);

                card.find('.cardlist-card-comments').append(newComment.clone());

                var commentsIcon = card.find('.cardlist-card-icon-comment');
                var commentCount = card.find('.cardlist-card-comments').find('.comment').length;
                commentsIcon.toggle(commentCount > 0);
                commentsIcon.find('.comment-count').text(commentCount);
            }
        });
    }
});

// Kártya nézetben komment törlése
$(document).on('click', '.comment-action-delete', function () {
    var cardId = $(this).closest('.card-details').data('id');
    var comment = $(this).closest('.comment');
    var commentId = comment.data('id');

    $.post('../../Card/RemoveComment', {
        cardId,
        commentId
    }).done(function (data) {
        if (data.success) {
            comment.remove();
            $('.comment[data-id=' + commentId + ']').remove();

            var card = $('.cardlist-card[data-id=' + cardId + ']');
            var commentsIcon = card.find('.cardlist-card-icon-comment');
            var commentCount = card.find('.cardlist-card-comments').find('.comment').length;
            commentsIcon.toggle(commentCount > 0);
            commentsIcon.find('.comment-count').text(commentCount);
        }
    });
});

/* ###############
   ## Functions ##
   ###############
*/

// Kártya nézet megjelenítése
function showCardDetails(caller) {
    var dimmer = $('<div>').addClass('dim-overlay');
    var cardDetails = $('.card-details').clone();
    var card = $(caller).closest('.cardlist-card');
    var cardName = card.find('.cardlist-card-name').text();
    var cardDesc = card.find('#card_Description').val();
    var commentDiv = card.find('.cardlist-card-comments').children().clone();

    cardDetails.attr('data-id', card.data('id'));
    cardDetails.find('.card-details-title').text(cardName);
    cardDetails.find('.card-details-description-text').text(cardDesc);
    cardDetails.find('.card-details-comments').append(commentDiv);

    $('body').append(dimmer).append(cardDetails);
}

// Kártya nézet elrejtése
function hideCardDetails() {
    $('.dim-overlay').remove();
    $('.card-details').last().remove();
}