/// <amd-module name="Core/Main" />
/// <amd-dependency name="jquery" />
import Core = require("Core/Common");
import Alert = require("Core/Alert");
import SpinShield = require("Core/SpinShield");
import $ = require("jquery");
import MarkdownDeepEditor = require("MarkdownDeepEditor");

export function Run()
{
    MarkdownDeepEditor; // Force markdowndeep dependency
    Core.generateFixedTables();
    $(window).on("resize", "", Core.generateFixedTables);
    $("textarea.mdd_editor").MarkdownDeep(Core.MarkdownEditorOptions);
}
