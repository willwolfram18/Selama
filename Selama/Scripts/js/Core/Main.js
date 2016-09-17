define("Core/Main", ["require", "exports", "Core/Common", "jquery", "MarkdownDeepEditor"], function (require, exports, Core, $, MarkdownDeepEditor) {
    "use strict";
    function Run() {
        MarkdownDeepEditor; // Force markdowndeep dependency
        Core.generateFixedTables();
        $(window).on("resize", "", Core.generateFixedTables);
        $("textarea.mdd_editor").MarkdownDeep(Core.MarkdownEditorOptions);
    }
    exports.Run = Run;
});
//# sourceMappingURL=Main.js.map