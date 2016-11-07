var Selama = Selama || {};
Selama.Core = Selama.Core || {};
Selama.Core.Alert = Selama.Core.Alert || {
    _defaultAlertTitle: "Alert",

    raiseAlert: function Selama_Core_Alert_RaiseAlert(text, title)
    {
        title = title || this._defaultAlertTitle;

        var $modal = this._initModalAndAttachToBody();
        $modal.find(".modal-body").text(text);
        $modal.find(".modal-title").text(title);
        $modal.modal("show");
    },

    _initModalAndAttachToBody: function Selama_Core_Alert_InitModalAndAttachToBody()
    {
        var $modal = this._createModal();
        $("body").append($modal);
        return $modal;
    },

    _createModal: function Selama_Core_Alert_CreateModal()
    {
        return Selama.Core.createElem("div", "modal fade")
            .attr("role", "dialog")
            .attr("data-backdrop", "static")
            .attr("data-keyboard", "false")
            .append(
                Selama.Core.createElem("div", "modal-dialog")
                    .append(
                        this._createModalContent()
                    )
            );
    },

    _createModalContent: function Selama_Core_Alert_CreateModalContent()
    {
        return Selama.Core.createElem("div", "modal-content")
            .attr("role", "document")
            .append(
                Selama.Core.createElem("div", "modal-header")
                    .append(
                        Selama.Core.createElem("button", "close")
                            .attr("type", "button")
                            .attr("data-dismiss", "modal")
                            .append(
                                Selama.Core.createElem("span")
                                    .html("&times;")
                            )
                    )
                    .append(
                        Selama.Core.createElem("h4", "modal-title")
                    )
            )
            .append(
                Selama.Core.createElem("div", "modal-body")
            )
            .append(
                Selama.Core.createElem("div", "modal-footer")
                    .append(
                        Selama.Core.createElem("div", "btn btn-primary")
                            .attr("type", "button")
                            .attr("data-dismiss", "modal")
                            .text("OK")
                    )
            )
        ;
    },
};