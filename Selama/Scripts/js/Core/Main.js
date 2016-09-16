define("Core/Main", ["require", "exports", "Core/Common", "jquery", "MarkdownDeep"], function (require, exports, Core, $, MarkdownDeep) {
    "use strict";
    function Run() {
        MarkdownDeep; // Force markdowndeep dependency
        Core.generateFixedTables();
        $(window).on("resize", "", Core.generateFixedTables);
        $("textarea.mdd_editor").MarkdownDeep(Core.MarkdownEditorOptions);
    }
    exports.Run = Run;
});
//# sourceMappingURL=Main.js.map