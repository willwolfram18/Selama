﻿var Selama = Selama || {};

Selama.Forums = Selama.Forums || {
    onAjaxRequestBegin: function Selama_Forums_OnAjaxRequestBegin()
    {
        Selama.SpinShield.raiseShield();
    },

    onAjaxRequestComplete: function Selama_Forums_OnAjaxRequestComplete()
    {
        Selama.SpinShield.lowerShield();
    },
};

Selama.Forums.Threads = Selama.Forums.Threads || {
    onThreadTitleMouseMove: function Selama_Forums_Threads_OnThreadTitleMouseMove(e)
    {
        /// <param name="e" type="jQuery.Event" />
        $(e.target).popover("show");
        $(".popover").css({ top: e.pageY - 14, left: e.pageX + 6 }).find(".arrow").css("top", "14px");
    },

    onThreadTitleMouseLeave: function Selama_Forums_Threads_OnThreadTitleMouseLeave(e)
    {
        /// <param name="e" type="jQuery.Event" />
        $(e.target).popover("hide");
    },
};

Selama.Forums.Thread = Selama.Forums.Thread || {
    onPostReplyClick: function onPostReplyClick()
    {
        $("#ThreadReplyEditor").show("blind")
            .find(".mdd_editor").focus();
    },

    onEditorModalShown: function Sealam_Forums_Thread_OnEditorModalShow(e)
    {
        /// <param name="e" type="jQuery.Event" />
        var $target = $(e.currentTarget);
        var $editor = $target.find("textarea.mdd_editor").focus();
        $("textarea.mdd_editor").MarkdownDeep(Selama.MarkdownEditor.Options);
        var m = new MarkdownDeepEditor.Editor($editor[0], $target.find("div.mdd_preview")[0]);
    },

    onQuoteBtnClick: function Selama_Forums_Thread_OnQuoteBtnClick(e)
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
            success: Selama.Forums.Thread.onQuoteBtnClick_Success,
        });
    },
    onQuoteBtnClick_Success: function Selama_Forums_Thread_OnQuoteBtnClick_Success(response)
    {
        var $editor = $("#ThreadReplyEditor textarea.mdd_editor");
        var currentVal = $editor.val().trim();
        Selama.Forums.Thread.onPostReplyClick(); // show the editor
        if (currentVal === "")
        {
            $editor.val(response).trigger("change");
        }
        else
        {
            $editor.val(currentVal + "\n\n" + response).trigger("change");
        }
        // Triggers transfromation of the content
        var m = new MarkdownDeepEditor.Editor($editor[0], $editor.next(".mdd_preview")[0]);
    },

    onDeleteFormSubmitClick: function Selama_Forums_Thread_OnDeletFormSubmitClick()
    {
        Selama.SpinShield.raiseShield();
    },

    onReplyDeleteClick: function Selama_Forums_Thread_OnReplyDeleteClick(e)
    {
        /// <param name="e" type="jQuery.Event" />
        // Set the hidden ID input for the reply
        $("#ReplyDeleteModal").find("input#id")
            .val(
                $(e.target).closest(".row.thread-reply").attr("data-thread-reply")
            ).trigger("change");
        $("#ReplyDeleteModal").modal("show");
    },
    onThreadDeleteClick: function Selama_Forums_Thread_OnThreadDeleteClick(e)
    {
        /// <param name="e" type="jQuery.Event" />
        $("#ThreadDeleteModal").modal("show");
    },

    onThreadUpdateRequestSuccess: function Selama_Forums_Thread_OnThreadUpdateRequestSuccess(response)
    {
        $(".row.thread[data-thread]").find(".thread-content").html(response);
        $("#ThreadEditModal").modal("hide");
    },
    onThreadUpdateRequestFailure: function Selama_Forums_Thread_OnThreadUpdateRequestFailure(response)
    {
        if (response.statusText === "Thread is locked")
        {
            Selama.Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
        }
        else
        {
            Selama.Alert.raiseAlert("An error occurred while updating");
        }
    },

    onReplyUpdateRequestSuccess: function Selama_Forums_Thread_OnReplyUpdateRequestSuccess(response)
    {
        $(".thread-reply[data-thread-reply='" + response.id + "']")
            .find(".thread-reply-content").html(response.content);
        $("#ThreadReplyEditModal").modal("hide");
    },
    onReplyUpdateRequestFailure: function Selama_Forums_Thread_OnReplyUpdateRequestFailure(response)
    {
        if (response.statusText === "Thread is locked")
        {
            Selama.Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
        }
        else
        {
            Selama.Alert.raiseAlert("An error occured while updating");
        }
    },

    onThreadEditLinkClick: function Selama_Forums_Thread_OnThreadEditLinkClick(e)
    {
        /// <param name="e" type="jQuery.Event" />
        $.ajax({
            url: e.data.url,
            type: 'GET',
            before: Selama.Forums.onAjaxRequestBegin,
            complete: Selama.Forums.onAjaxRequestComplete,
            success: Selama.Forums.Thread.onThreadEditLinkClick_Success,
            error: Selama.Forums.Thread.onThreadEditLinkClick_Error,
        });
    },
    onThreadEditLinkClick_Success: function Selama_Forums_Thread_OnThreadEditLinkClick_Success(response)
    {
        $("#ThreadEditModal").modal("show").find(".modal-body").html(response);
    },
    onThreadEditLinkClick_Error: function Selama_Fourms_Thread_OnThreadEditLinkClick_Error(response)
    {
        if (response.statusText === "Invalid ID")
        {
            Selama.Alert.raiseAlert("An invalid thread ID was provided", "Invalid ID");
        }
        else if (response.statusText === "Thread is locked")
        {
            Selama.Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
        }
        else
        {
            Selama.Alert.raiseAlert("An error occurred");
        }
    },

    onReplyEditLinkClick: function Selama_Forums_Thread_OnReplyEditLinkClick(e)
    {
        /// <param name="e" type="jQuery.Event" />
        var replyId = $(e.target).closest(".row[data-thread-reply]").attr("data-thread-reply");

        $.ajax({
            url: e.data.url,
            data: { id: replyId },
            type: 'GET',
            before: Selama.Forums.onAjaxRequestBegin,
            complete: Selama.Forums.onAjaxRequestComplete,
            success: Selama.Forums.Thread.onReplyEditLinkClick_Success,
            error: Selama.Forums.Thread.onReplyEditLinkClick_Error,
        });
    },
    onReplyEditLinkClick_Success: function Selama_Forums_Thread_OnReplyEditLinkClick_Success(response)
    {
        $("#ThreadReplyEditModal").modal("show").find(".modal-body").html(response);
    },
    onReplyEditLinkClick_Error: function Selama_Forums_Thread_OnReplyEditLinkClick_Error(response)
    {
        if (response.statusText === "Invalid ID")
        {
            Selama.Alert.raiseAlert("An invalid thread reply ID was provided", "Invalid ID");
        }
        else if (response.statusText === "Thread is locked")
        {
            Selama.Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
        }
        else
        {
            Selama.Alert.raiseAlert("An error occurred");
        }
    },
};