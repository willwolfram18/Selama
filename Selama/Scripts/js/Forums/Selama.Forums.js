define("Forums/Selama.Forums", ["require", "exports", "Core/Selama.Core.SpinShield"], function (require, exports, SpinShield) {
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
//# sourceMappingURL=Selama.Forums.js.map