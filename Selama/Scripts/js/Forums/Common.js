define("Forums/Common", ["require", "exports", "Core/SpinShield"], function (require, exports, SpinShield) {
    "use strict";
    function onAjaxRequestBegin() {
        console.log("onAjaxRequestBegin");
        SpinShield.raiseShield();
    }
    exports.onAjaxRequestBegin = onAjaxRequestBegin;
    function onAjaxRequestComplete() {
        console.log("onAjaxRequestEnd");
        SpinShield.lowerShield();
    }
    exports.onAjaxRequestComplete = onAjaxRequestComplete;
});
//# sourceMappingURL=Common.js.map