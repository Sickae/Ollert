// Új kategória hozzáadása
$(document).on('click', '.category-new', function () {
    $.post('../../Category/AddNewCategory', {
        categoryName: 'TEST_CATEGORY'
    }).done(function (data) {
        if (data.success) {
            console.log(data.category);
        }
    });
}); 