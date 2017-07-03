function getValueFromUrl(url, key) {
    var value = null;
    var tokens = url.substr(url.indexOf('#') + 1).split('&');
    for (var _i = 0, tokens_1 = tokens; _i < tokens_1.length; _i++) {
        var t = tokens_1[_i];
        if (t.match("^" + key + "=.+$")) {
            value = t.substr(key.length + 1);
            value = decodeURIComponent(value);
        }
    }
    return value;
}
function getPlayerId() {
    return getValueFromUrl(location.href, "id");
}
function getScoreUrl() {
    return getValueFromUrl(location.href, "gameScoreUrl");
}
function botGameScoreUrlExists() {
    var scoreUrl = getScoreUrl();
    return scoreUrl !== null && scoreUrl.toString().length > 1;
}
//# sourceMappingURL=TgBF.js.map