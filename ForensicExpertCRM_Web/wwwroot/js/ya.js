// Инициализируем автодополнение
ymaps.ready(function () {
    var suggestView = new ymaps.SuggestView('suggest', { results: 5 });

    // При выборе адреса из списка
    suggestView.events.add('select', function (e) {
        // Получаем выбранный адрес
        var address = e.get('item').value;
        // Выводим выбранный адрес в поле ввода
        $('#suggest').val(address);
        // Очищаем список предложений
        $('#suggest-list').html('');
    });

    // При вводе текста в поле ввода
    suggestView.events.add('suggest', function (e) {
        // Получаем список предложений
        var suggest = e.get('suggest');
        // Очищаем список предложений
        $('#suggest-list').html('');

        // Добавляем каждый элемент списка в выпадающий список
        suggest.forEach(function (item) {
            $('#suggest-list').append('<option value="' + item.value + '" />');
        });
    });
});


