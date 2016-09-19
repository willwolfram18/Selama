define("Forums/Thread", ["require", "exports", "jquery", "Core/Alert", "Core/Common", "Forums/Common", "Core/SpinShield"], function (require, exports, $, Alert, Common, Forums, SpinShield) {
    "use strict";
    var MarkdownDeepEditor;
    function Setup(mdEditor) {
        MarkdownDeepEditor = mdEditor;
    }
    exports.Setup = Setup;
    function onPostReplyClick() {
        $("#ThreadReplyEditor").show("blind")
            .find(".mdd_editor").focus();
    }
    exports.onPostReplyClick = onPostReplyClick;
    function onEditorModalShown(e) {
        var $target = $(e.target);
        var $editor = $target.find("textarea.mdd_editor").focus();
        $("textarea.mdd_editor").MarkdownDeep(Common.MarkdownEditorOptions);
        var editorObj = new MarkdownDeepEditor.Editor($editor[0], $target.find("div.mdd_preview")[0]);
    }
    exports.onEditorModalShown = onEditorModalShown;
    function onQuoteBtnClick(e) {
        var replyQuoteUrl = e.data.replyUrl;
        var threadQuoteUrl = e.data.threadUrl;
        var $threadPost = $(e.target).closest(".row.thread-post");
        var ajaxId = 0;
        var ajaxUrl = "";
        if ($threadPost.is(".thread")) {
            ajaxId = +$threadPost.attr("data-thread");
            ajaxUrl = threadQuoteUrl;
        }
        else if ($threadPost.is(".thread-reply")) {
            ajaxId = +$threadPost.attr("data-thread-reply");
            ajaxUrl = replyQuoteUrl;
        }
        else {
            throw new Error("Invalid thread post type");
        }
        $.ajax({
            url: ajaxUrl,
            data: { id: ajaxId },
        }).then(function (response, status, jqXhr) {
            // Use of the anonymous function to guarantee MarkdownDeepEditor.Editor class
            if (status === "success") {
                onQuoteBtnClick_Success(response, status, jqXhr);
            }
        });
    }
    exports.onQuoteBtnClick = onQuoteBtnClick;
    function onQuoteBtnClick_Success(response, status, jqXhr) {
        var $editor = $("#ThreadReplyEditor textarea.mdd_editor");
        var currentVal = $editor.val().trim();
        onPostReplyClick();
        if (currentVal === "") {
            $editor.val(response);
        }
        else {
            $editor.val(currentVal + "\n\n" + response);
        }
        $editor.trigger("change");
        var editorMarkdown = new MarkdownDeepEditor.Editor($editor[0], $editor.next(".mdd_preview")[0]);
    }
    function onDeleteFormSubmitClick(e) {
        SpinShield.raiseShield();
    }
    exports.onDeleteFormSubmitClick = onDeleteFormSubmitClick;
    function onReplyDeleteClick(e) {
        var threadReplyId = $(e.target).closest(".row.thread-reply").attr("data-thread-reply");
        var $replyDeleteModal = $("#ReplyDeleteModal");
        $replyDeleteModal.find("input#id")
            .val(threadReplyId).trigger("change");
        $replyDeleteModal.modal("show");
    }
    exports.onReplyDeleteClick = onReplyDeleteClick;
    function onThreadDeleteClick(e) {
        $("#ThreadDeleteModal").modal("show");
    }
    exports.onThreadDeleteClick = onThreadDeleteClick;
    function onThreadUpdateRequest_Success(response, status, jqXhr) {
        $(".row.thread[data-thread]").find(".thread-content").html(response);
        $("#ThreadEditModal").modal("hide");
    }
    exports.onThreadUpdateRequest_Success = onThreadUpdateRequest_Success;
    function onThreadUpdateRequest_Failure(response) {
        if (response.statusText === "Thread is locked") {
            Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
        }
        else {
            Alert.raiseAlert("An unexpected error occurred while updating the thread");
        }
    }
    exports.onThreadUpdateRequest_Failure = onThreadUpdateRequest_Failure;
    function onReplyUpdateRequest_Success(response, status, jqXhr) {
        $(".thread-reply[data-thread-reply='" + response.id + "']")
            .find(".thread-reply-content").html(response.content);
        $("#ThreadReplyEditModal").modal("hide");
    }
    exports.onReplyUpdateRequest_Success = onReplyUpdateRequest_Success;
    function onReplyUpdateRequest_Failure(response) {
        if (response.statusText === "Thread is locked") {
            Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
        }
        else {
            Alert.raiseAlert("An unexpected error occurred while updating");
        }
    }
    exports.onReplyUpdateRequest_Failure = onReplyUpdateRequest_Failure;
    function onThreadEditLinkClick(e) {
        $.ajax({
            url: e.data.url,
            type: "GET",
            beforeSend: Forums.onAjaxRequestBegin,
            complete: Forums.onAjaxRequestComplete,
            success: onThreadEditLinkClick_Success,
            error: onThreadEditLinkClick_Failure,
        });
    }
    exports.onThreadEditLinkClick = onThreadEditLinkClick;
    function onThreadEditLinkClick_Success(response, status, jqXhr) {
        $("#ThreadEditModal").modal("show").find(".modal-body").html(response);
    }
    function onThreadEditLinkClick_Failure(response) {
        if (response.statusText === "Invalid ID") {
            Alert.raiseAlert("An invalid thread ID was provided", "Invalid Thread");
        }
        else if (response.statusText === "Thread is locked") {
            Alert.raiseAlert("The thread is locked for editing", "Thread is locked");
        }
        else {
            Alert.raiseAlert("An unexpected error occurred");
        }
    }
    function onReplyEditLinkClick(e) {
        var replyId = $(e.target).closest(".row[data-thread-reply]").attr("data-thread-reply");
        $.ajax({
            url: e.data.url,
            data: { id: replyId },
            type: "GET",
            beforeSend: Forums.onAjaxRequestBegin,
            complete: Forums.onAjaxRequestComplete,
            success: onReplyEditLinkClick_Success,
            error: onReplyEditLinkClick_Failure
        });
    }
    exports.onReplyEditLinkClick = onReplyEditLinkClick;
    function onReplyEditLinkClick_Success(response, status, jqXhr) {
        $("#ThreadReplyEditModal").modal("show").find(".modal-body").html(response);
    }
    function onReplyEditLinkClick_Failure(response) {
        if (response.statusText === "Invalid ID") {
            Alert.raiseAlert("An invalid thread reply ID was provided", "Invalid thread reply");
        }
        else if (response.statusText === "Thread is locked") {
            Alert.raiseAlert("The thread is locked for editing", "Thread is locked");
        }
        else {
            Alert.raiseAlert("An unexpected error occurred");
        }
    }
});
//# sourceMappingURL=Thread.js.map