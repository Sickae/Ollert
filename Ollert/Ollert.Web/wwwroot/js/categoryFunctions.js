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
            var emptyCategory = $('.templates > .empty-category > .category').clone();
            var newBoard = $('.templates > .empty-category > ul').clone();

            categoryNewContainer.find('.category-new').show();
            categoryNewContainer.find('.category-new-input-container').hide();
            emptyCategory.find('.category-name').val(name);
            emptyCategory.attr('data-id', data.id);
            emptyCategory.find('.category-name').attr('data-name', name);

            $('.board-list').append(emptyCategory).append(newBoard).append(categoryNewContainer);
        }
    });
}