System.register(["Selama.Core"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Core;
    var _defaultAlertTitle;
    function raiseAlert(text, title) {
        if (title === undefined) {
            title = _defaultAlertTitle;
        }
        var $modal = _initModalAndAttachToBody();
        $modal.find(".modal-body").text(text);
        $modal.find(".modal-title").text(title);
        $modal.modal("show");
    }
    exports_1("raiseAlert", raiseAlert);
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
    return {
        setters:[
            function (Core_1) {
                Core = Core_1;
            }],
        execute: function() {
            _defaultAlertTitle = "Alert";
        }
    }
});
//# sourceMappingURL=Selama.Core.Alert.js.map