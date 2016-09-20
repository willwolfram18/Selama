define("Core/Main", ["require", "exports", "Core/Common", "jquery", "MarkdownDeepEditor"], function (require, exports, Common, $, MarkdownDeepEditor) {
    "use strict";
    function Run() {
        MarkdownDeepEditor; // Force markdowndeep dependency
        Common.generateFixedTables();
        $(window).on("resize", "", Common.generateFixedTables);
        $("textarea.mdd_editor").MarkdownDeep(Common.MarkdownEditorOptions);
    }
    exports.Run = Run;
});
//# sourceMappingURL=Main.js.map