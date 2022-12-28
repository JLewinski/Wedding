// const exports = {};
function require(strId) {
    if (window[strId]) {
        return window[strId];
    } else if(strId.toLowerCase() == 'jquery') {
        return window.jQuery;
    } else {
        return window;
    }
}