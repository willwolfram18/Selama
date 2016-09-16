/// <amd-module name="Forums/Thread" />
import $ = require("jquery");
import MarkdownDeep = require("MarkdownDeep");

function onPostReplyClick(): void
{
    $("#ThreadReplyEditor").show("blind")
        .find(".mdd_editor").focus();
}

function onEditorModalShown(e: JQueryEventObject): void
{
    var $target: JQuery = $(e.target);
    var $editor: JQuery = $target.find("textarea.mdd_editor").focus();
    $("textarea.mdd_editor").MarkdownDeep();
}