define("Forums/Thread", ["require", "exports", "jquery"], function (require, exports, $) {
    "use strict";
    function onPostReplyClick() {
        $("#ThreadReplyEditor").show("blind")
            .find(".mdd_editor").focus();
    }
    function onEditorModalShown(e) {
        var $target = $(e.target);
        var $editor = $target.find("textarea.mdd_editor").focus();
        $("textarea.mdd_editor").MarkdownDeep();
    }
});
//# sourceMappingURL=Thread.js.map