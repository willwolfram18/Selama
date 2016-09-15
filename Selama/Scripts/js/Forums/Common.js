define("Forums/Common", ["require", "exports", "Core/SpinShield"], function (require, exports, SpinShield) {
    "use strict";
    function onAjaxRequestBegin() {
        console.log("onAjaxRequestBegin");
        SpinShield.raiseShield();
    }
    exports.onAjaxRequestBegin = onAjaxRequestBegin;
    function onAjaxRequestEnd() {
        console.log("onAjaxRequestEnd");
        SpinShield.lowerShield();
    }
    exports.onAjaxRequestEnd = onAjaxRequestEnd;
});
//# sourceMappingURL=Common.js.map