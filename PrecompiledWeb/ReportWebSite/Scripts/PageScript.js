
function AddWndHeightToCallbackParam(param) {

    return "" + param;

    /*
    var winW = 1024;
    var winH = 768;

    if (document.body && document.body.offsetWidth) {
        winW = document.body.offsetWidth;
        winH = document.body.offsetHeight;
    }

    if (document.compatMode == 'CSS1Compat' &&
            document.documentElement &&
            document.documentElement.offsetWidth) {

        winW = document.documentElement.offsetWidth;
        winH = document.documentElement.offsetHeight;
    }

    if (window.innerWidth && window.innerHeight) {
        winW = window.innerWidth;
        winH = window.innerHeight;
    }

    return "[" + winH + "]:" + param;
    */
}