var Selama = Selama || {};

Selama.Forums = Selama.Forums || {
    onEditorModalShown: function Sealam_Forums_OnEditorModalShow(e)
    {
        /// <param name="e" type="jQuery.Event" />
        var $target = $(e.currentTarget);
        var $editor = $target.find("textarea.mdd_editor").focus();
        $("textarea.mdd_editor").MarkdownDeep(Selama.MarkdownEditor.Options);
        var m = new MarkdownDeepEditor.Editor($editor[0], $target.find("div.mdd_preview")[0]);
    },

    onQuoteBtnClick: function Selama_Forums_OnQuoteBtnClick(e)
    {
        /// <param name="e" type="jQuery.Event" />
        // Set both urls, we don't know which one we'll need
        var replyQuoteUrl = e.data.replyUrl;
        var threadQuoteUrl = e.data.threadUrl;

        var $threadPost = $(e.target).closest(".row.thread-post");
        var ajaxId = 0;
        var ajaxUrl = "";

        if ($threadPost.is(".thread"))
        {
            ajaxId = $threadPost.attr("data-thread");
            ajaxUrl = threadQuoteUrl;
        }
        else if ($threadPost.is(".thread-reply"))
        {
            ajaxId = $threadPost.attr("data-thread-reply")
            ajaxUrl = replyQuoteUrl;
        }

        $.ajax({
            url: ajaxUrl,
            data: { id: ajaxId },
            success: onQuoteBtnClick_Success,
        });
    },
};