$(document).ready(function () {
    $('pre code').each(function (i, block) {
        hljs.highlightBlock(block);
    });

    $('.btnBack').unbind('click').bind('click', function () {
        //window.history.back();
        window.location.href = window.location.origin;
    });

});