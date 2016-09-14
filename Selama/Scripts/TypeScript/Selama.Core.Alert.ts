/// <amd-module name="Selama.Core.Alert" />
import Core = require("Selama.Core");

let _defaultAlertTitle = "Alert";

export function raiseAlert(text: string, title?: string)
{
    if (title === undefined)
    {
        title = _defaultAlertTitle;
    }

    let $modal = _initModalAndAttachToBody();

    $modal.find(".modal-body").text(text);
    $modal.find(".modal-title").text(title);
    $modal.modal("show");
}

function _initModalAndAttachToBody(): JQuery
{
    let $modal = _createModal();
    $("body").append($modal);
    return $modal;
}
function _createModal(): JQuery
{
    return Core.createElem("div", "modal fade")
        .attr("role", "dialog")
        .attr("data-backdrop", "static")
        .attr("data-keyboard", "false")
        .append(
        Core.createElem("div", "modal-dialog")
            .append(
                _createModalContent()
            )
        );
}
function _createModalContent(): JQuery
{
    return Core.createElem("div", "modal-content")
        .append(
        Core.createElem("div", "modal-header")
            .append(
            Core.createElem("button", "close")
                .attr("type", "button")
                .attr("data-dismiss", "modal")
                .html("&times;")
            )
            .append(
            Core.createElem("h4", "modal-title")
            )
        )
        .append(
        Core.createElem("div", "modal-body")
        )
        .append(
        Core.createElem("div", "modal-footer")
            .append(
            Core.createElem("button", "btn btn-primary")
                .attr("type", "button")
                .attr("data-dismiss", "modal")
                .text("OK")
            )
        );
}
