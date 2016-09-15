/// <amd-module name="Core/Selama.Core.Alert" />
/// <amd-dependency name="jquery" />
/// <amd-dependency name="bootstrap" />
/// <reference path="../typings/bootstrap/bootstrap.d.ts" />
import $ = require("jquery");
import bootstrap = require("bootstrap");
import Core = require("Core/Selama.Core");

let _defaultAlertTitle: string = "Alert";

export function raiseAlert(text: string, title?: string): void
{
    bootstrap; // hack to force bootstrap as a required module
    if (title === undefined)
    {
        title = _defaultAlertTitle;
    }

    let $modal: JQuery = _initModalAndAttachToBody();

    $modal.find(".modal-body").text(text);
    $modal.find(".modal-title").text(title);
    $modal.modal("show");
}

function _initModalAndAttachToBody(): JQuery
{
    let $modal: JQuery = _createModal();
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
