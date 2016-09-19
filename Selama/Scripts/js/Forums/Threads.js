define("Forums/Threads", ["require", "exports", "jquery", "bootstrap"], function (require, exports, $, bootstrap) {
    "use strict";
    function Setup() {
        bootstrap; // hacky way to force bootstrap dependency
        $("td.thread-title").on("mousemove", onThreadTitleMouseMove)
            .on("mouseleave", onThreadTitleMouseLeave)
            .each(initPopover);
        $("[data-toggle='tooltip']").tooltip();
    }
    exports.Setup = Setup;
    function onThreadTitleMouseMove(e) {
        var $target = $(e.target);
        if ($target.is("a")) {
            // Update target to point to thread title parent
            e.target = $target.closest("td.thread-title")[0];
            onThreadTitleMouseLeave(e);
            return;
        }
        var pageYOffset = -14;
        var pageXOffest = 6;
        var arrowTopPos = "14px";
        $target.popover("show");
        // offset popover position to reduce flashing on mousemove
        // adjust .arrow top to align with cursor position
        $(".popover").css({ top: e.pageY + pageYOffset, left: e.pageX + pageXOffest })
            .find(".arrow").css("top", arrowTopPos);
    }
    function onThreadTitleMouseLeave(e) {
        var $target = $(e.target);
        $target.popover("hide");
    }
    function initPopover(index, popoverElem) {
        var popoverOptions = {
            animation: false,
            container: "body",
            html: true,
            placement: "right",
            trigger: "manual"
        };
        $(popoverElem).popover(popoverOptions);
    }
});
//# sourceMappingURL=Threads.js.map