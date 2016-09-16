define("Forums/Thread", ["require", "exports", "jquery", "MarkdownDeep", "MarkdownDeepEditor"], function (require, exports, $, MarkdownDeep, MarkdownDeepEditor) {
    "use strict";
    var Selama = Selama || {};
    function onPostReplyClick() {
        $("#ThreadReplyEditor").show("blind")
            .find(".mdd_editor").focus();
    }
    function onEditorModalShown(e) {
        MarkdownDeep; // force MarkdownDeep into dependency statement
        var $target = $(e.target);
        var $editor = $target.find("textarea.mdd_editor").focus();
        $("textarea.mdd_editor").MarkdownDeep(Selama.MarkdownEditor.Options);
        var editorObj = new MarkdownDeepEditor.Editor($editor[0], $target.find("div.mdd_preview")[0]);
    }
});
//# sourceMappingURL=Thread.js.map