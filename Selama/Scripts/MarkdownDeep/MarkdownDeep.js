var Selama = Selama || {};
Selama.MarkdownEditor = Selama.MarkdownEditor || {};
Selama.MarkdownEditor.Options = Selama.MarkdownEditor.Options || {
    resizebar: false,
    SafeMode: true,
    help_location: Selama.MarkdownEditor.HelpLocation
}

$(document).ready(function ()
{
    $("textarea.mdd_editor").MarkdownDeep(Selama.MarkdownEditor.Options);
})
