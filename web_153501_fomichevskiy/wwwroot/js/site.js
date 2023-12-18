$('a.page-link').click(function (e) {
        e.preventDefault(); 

        var url = $(this).attr('href'); 

    $("#partial-content").load(url)


        //$.get(url, function (data) {
        //    $('#partial-content').html(data);
        //});
    });