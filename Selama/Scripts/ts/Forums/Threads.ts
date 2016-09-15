/// <amd-module name="Forums/Threads" />
import $ = require("jquery");
import bootstrap = require("bootstrap");

export function Run()
{
    bootstrap; // hacky way to force bootstrap dependency
    $("td.thread-title").on("mousemove", onThreadTitleMouseMove)
        .on("mouseleave", onThreadTitleMouseLeave)
        .each(initPopover);
    $("[data-toggle='tooltip']").tooltip();
}

function onThreadTitleMouseMove(e: JQueryMouseEventObject): void
{
    let pageYOffset: number = -14;
    let pageXOffest: number = 6;
    let arrowTopPos: string = "14px";
    $(e.target).popover("show");
    // offset popover position to reduce flashing on mousemove
    // adjust .arrow top to align with cursor position
    $(".popover").css({ top: e.pageY + pageYOffset, left: e.pageX + pageXOffest })
        .find(".arrow").css("top", arrowTopPos);
}

function onThreadTitleMouseLeave(e: JQueryMouseEventObject): void
{
    $(e.target).popover("hide");
}

function initPopover(index: number, popoverElem: Element): any
{
    let popoverOptions: PopoverOptions = {
        animation: false,
        container: "body",
        html: true,
        placement: "right",
        trigger: "manual"
    };
    $(popoverElem).popover(popoverOptions);
}