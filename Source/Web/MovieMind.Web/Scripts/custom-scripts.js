function showText(id) {
    if (document.getElementById('content' + id).style.display == "none") {
        $('#label' + id).attr('data-attr', '-');
        document.getElementById('content' + id).style.display = "block";
    } else {
        $('#label' + id).attr('data-attr', '+');
        document.getElementById('content' + id).style.display = "none";
    }
}

function changePage(page) {
    $('#p').val(page);
    $("#searchForm").submit();
}