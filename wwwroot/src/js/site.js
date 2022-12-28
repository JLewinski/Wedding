const exports = {};
function require(strId) {
    if (window[strId]) {
        return window[strId];
    } else {
        return window;
    }
}