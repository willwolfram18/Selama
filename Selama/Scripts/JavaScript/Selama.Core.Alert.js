define("Selama.Core.Alert", ["require", "exports", "./Selama.Core"], function (require, exports, Core) {
    "use strict";
    var _defaultAlertTitle = "Alert";
    function raiseAlert(text, title) {
        if (title === undefined) {
            title = _defaultAlertTitle;
        }
        var $modal = _initModalAndAttachToBody();
        $modal.find(".modal-body").text(text);
        $modal.find(".modal-title").text(title);
        $modal.modal("show");
    }
    exports.raiseAlert = raiseAlert;
    function _initModalAndAttachToBody() {
        var $modal = _createModal();
        $("body").append($modal);
        return $modal;
    }
    function _createModal() {
        return Core.createElem("div", "modal fade")
            .attr("role", "dialog")
            .attr("data-backdrop", "static")
            .attr("data-keyboard", "false")
            .append(Core.createElem("div", "modal-dialog")
            .append(_createModalContent()));
    }
    function _createModalContent() {
        return Core.createElem("div", "modal-content")
            .append(Core.createElem("div", "modal-header")
            .append(Core.createElem("button", "close")
            .attr("type", "button")
            .attr("data-dismiss", "modal")
            .html("&times;"))
            .append(Core.createElem("h4", "modal-title")))
            .append(Core.createElem("div", "modal-body"))
            .append(Core.createElem("div", "modal-footer")
            .append(Core.createElem("button", "btn btn-primary")
            .attr("type", "button")
            .attr("data-dismiss", "modal")
            .text("OK")));
    }
});
//# sourceMappingURL=Selama.Core.Alert.js.map