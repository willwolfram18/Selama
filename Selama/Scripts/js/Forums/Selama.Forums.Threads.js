define("Forums/Selama.Forums.Threads", ["require", "exports", "jquery"], function (require, exports, $) {
    "use strict";
    function onThreadTitleMouseMove(e) {
        var pageYOffset = -14;
        var pageXOffest = 6;
        var arrowTopPos = "14px";
        $(e.target).popover("show");
        // offset popover position to reduce flashing on mousemove
        // adjust .arrow top to align with cursor position
        $(".popover").css({ top: e.pageY + pageYOffset, left: e.pageX + pageXOffest })
            .find(".arrow").css("top", arrowTopPos);
    }
    exports.onThreadTitleMouseMove = onThreadTitleMouseMove;
    function onThreadTitleMouseLeave(e) {
        $(e.target).popover("hide");
    }
    exports.onThreadTitleMouseLeave = onThreadTitleMouseLeave;
});
//# sourceMappingURL=Selama.Forums.Threads.js.map