
    $(document).on('click', '.articleSearchBtn', function () {
           // window.alert("TESTING1");
            var query = $('#articleSearch').val();
    $.get('/News/SearchForArticle', {query: query }, function (data) {
        $('.ultimate-container').html(data);
            })


        });


    $(document).on('keypress','#articleSearch',function(e){
            if (e.which == 13) {
                var query = $('#articleSearch').val();

    $.get('/News/SearchForArticle', {query: query }, function (data) {
        $('.ultimate-container').html(data);
                })

            }

        })


