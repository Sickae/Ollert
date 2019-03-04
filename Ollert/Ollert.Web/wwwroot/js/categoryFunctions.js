 /* ############
   ## Events ##
   ############
*/
 
 // Új kategória hozzáadásnál Enter és Esc gombok működése
$(document).on('keyup', '.category-new-input', function (event) {
    if (event.keyCode === 13) {
        addNewCategory();
    } else if (event.keyCode === 27) {
        toggleNewCategoryWindow();
    }
});

// Kategória törlése ikonra kattintva
$(document).on('click', '.category-icon-delete', function () {
    var categoryContainer = $(this).closest('.category-container');
    var categoryId = categoryContainer.find('.category').data('id');

    $.post('../../Category/RemoveCategory', {
        id: categoryId
    }).done(function (data) {
        if (data.success) {
            categoryContainer.remove();
        }
    });
});

// Kategória átnevezése név mező fókusz vesztésekor
$(document).on('blur', '.category-name', function () {
    var name = $(this).val();

    if (name.length === 0) {
        $(this).val($(this).data('name'));
    } else {
        var id = $(this).closest('.category').data('id');

        $.post('../../Category/Rename', {
            id,
            name
        }).done(function (data) {
            if (data.success) {
                $(this).attr('data-name', name);
            }
        });
    }
});

// Kategória név mezőnél Enter és Esc gombok működése 
$(document).on('keydown', '.category-name', function (event) {
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
 
 // Új kategória hozzáadása ablak ki/be kapcsolása
function toggleNewCategoryWindow() {
    $('input.category-new-input').val('');
    $('.category-new').toggle();
    var inputContainer = $('.category-new-input-container').slideToggle('fast');

    if (inputContainer.css('display') === 'block') {
        $('.category-new-input').focus();
    }
}

// Új kategória hozzáadása
function addNewCategory() {
    var categoryNewContainer = $('.category-new-container');
    var name = categoryNewContainer.find('input.category-new-input').val();

    $.post('../../Category/AddNewCategory', {
        categoryName: name
    }).done(function (data) {
        if (data.success) {
            var emptyCategoryContainer = $('.templates > .empty-category > .category-container').clone();

            categoryNewContainer.find('.category-new').show();
            categoryNewContainer.find('.category-new-input-container').hide();
            emptyCategoryContainer.find('.category').attr('data-id', data.id);
            emptyCategoryContainer.find('.category-name').val(name).attr('data-name', name);
            emptyCategoryContainer.find('.board-list-item-new > a').attr('href', '../../Board/New?categoryId=' + data.id);

            $('.board-list').append(emptyCategoryContainer).append(categoryNewContainer);
        }
    });
}