/// <amd-module name="Forums/Thread" />
import $ = require("jquery");
import MarkdownDeep = require("MarkdownDeep");
import MarkdownDeepEditor = require("MarkdownDeepEditor");

var Selama = Selama || {};

function onPostReplyClick(): void
{
    $("#ThreadReplyEditor").show("blind")
        .find(".mdd_editor").focus();
}

function onEditorModalShown(e: JQueryEventObject): void
{
    MarkdownDeep; // force MarkdownDeep into dependency statement
    let $target: JQuery = $(e.target);
    let $editor: JQuery = $target.find("textarea.mdd_editor").focus();
    $("textarea.mdd_editor").MarkdownDeep(Selama.MarkdownEditor.Options);
    let editorObj: MarkdownDeepEditor.Editor = new MarkdownDeepEditor.Editor($editor[0], $target.find("div.mdd_preview")[0]);
}