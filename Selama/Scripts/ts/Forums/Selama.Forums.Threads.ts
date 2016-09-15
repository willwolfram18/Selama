/// <amd-module name="Forums/Selama.Forums.Threads" />
import $ = require("jquery");
import bootstrap = require("bootstrap");

export function onThreadTitleMouseMove(e: JQueryMouseEventObject): void
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

export function onThreadTitleMouseLeave(e: JQueryMouseEventObject): void
{
    $(e.target).popover("hide");
}