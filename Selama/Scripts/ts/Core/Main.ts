/// <amd-module name="Core/Main" />
/// <amd-dependency name="jquery" />
import Common = require("Core/Common");
import Alert = require("Core/Alert");
import SpinShield = require("Core/SpinShield");
import $ = require("jquery");
import MarkdownDeepEditor = require("MarkdownDeepEditor");

export function Run()
{
    MarkdownDeepEditor; // Force markdowndeep dependency
    Common.generateFixedTables();
    $(window).on("resize", "", Common.generateFixedTables);
    $("textarea.mdd_editor").MarkdownDeep(Common.MarkdownEditorOptions);
}
