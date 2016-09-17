/// <amd-module name="Forums/Thread" />
/// <reference path="../typings/markdowndeep/MarkdownDeepEditor.d.ts" />
import $ = require("jquery");
import Alert = require("Core/Alert");
import Common = require("Core/Common");
import Forums = require("Forums/Common");
import SpinShield = require("Core/SpinShield");

var MarkdownDeepEditor: any;

export function Setup(mdEditor: any): void
{
    MarkdownDeepEditor = mdEditor;
}

function onPostReplyClick(): void
{
    $("#ThreadReplyEditor").show("blind")
        .find(".mdd_editor").focus();
}

function onEditorModalShown(e: JQueryEventObject): void
{
    let $target: JQuery = $(e.target);
    let $editor: JQuery = $target.find("textarea.mdd_editor").focus();
    $("textarea.mdd_editor").MarkdownDeep(Common.MarkdownEditorOptions);
    let editorObj: Editor = new MarkdownDeepEditor.Editor($editor[0], $target.find("div.mdd_preview")[0]);
}

export function onQuoteBtnClick(e: JQueryEventObject): void
{
    let replyQuoteUrl: string = e.data.replyUrl;
    let threadQuoteUrl: string = e.data.threadUrl;

    let $threadPost: JQuery = $(e.target).closest(".row.thread-post");
    let ajaxId: number = 0;
    let ajaxUrl: string = "";

    if ($threadPost.is(".thread"))
    {
        ajaxId = +$threadPost.attr("data-thread");
        ajaxUrl = threadQuoteUrl;
    }
    else if ($threadPost.is(".thread-reply"))
    {
        ajaxId = +$threadPost.attr("data-thread-reply");
        ajaxUrl = replyQuoteUrl;
    }
    else
    {
        throw new Error("Invalid thread post type");
    }
    
    $.ajax({
        url: ajaxUrl,
        data: { id: ajaxId },
    }).then((response: string, status: string, jqXhr: JQueryXHR) =>
    {
        // Use of the anonymous function to guarantee MarkdownDeepEditor.Editor class
        if (status === "success")
        {
            onQuoteBtnClick_Success(response, status, jqXhr);
        }
    });
}
function onQuoteBtnClick_Success(response: string, status: string, jqXhr: JQueryXHR): void
{
    let $editor: JQuery = $("#ThreadReplyEditor textarea.mdd_editor");
    let currentVal: string = $editor.val().trim();
    onPostReplyClick();
    if (currentVal === "")
    {
        $editor.val(response);
    }
    else
    {
        $editor.val(currentVal + "\n\n" + response);
    }
    $editor.trigger("change");
    var editorMarkdown: Editor = new MarkdownDeepEditor.Editor($editor[0], $editor.next(".mdd_preview")[0]);
}

export function onDeleteFormSubmitClick(e: JQueryEventObject): void
{
    SpinShield.raiseShield();
}

export function onReplyDeleteClick(e: JQueryEventObject): void
{
    let threadReplyId: string = $(e.target).closest(".row.thread-reply").attr("data-thread-reply");
    let $replyDeleteModal: JQuery = $("#ReplyDeleteModal");
    $replyDeleteModal.find("input#id")
        .val(threadReplyId).trigger("change");
    $replyDeleteModal.modal("show");
}

export function onThreadDeleteClick(e: JQueryEventObject): void
{
    $("#ThreadDeleteModal").modal("show");
}

function onThreadUpdateRequest_Success(response: string, status: string, jqXhr: JQueryXHR): void
{
    $(".row.thread[data-thread]").find(".thread-content").html(response);
    $("#ThreadEditModal").modal("hide");
}
function onThreadUpdateRequest_Failure(response: JQueryXHR): void
{
    if (response.statusText === "Thread is locked")
    {
        Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
    }
    else
    {
        Alert.raiseAlert("An unexpected error occurred while updating the thread");
    }
}

function onReplyUpdateRequest_Success(response: any, status: string, jqXhr: JQueryXHR): void
{
    $(".thread-reply[data-thread-reply='" + response.id + "']")
        .find(".thread-reply-content").html(response.content);
    $("#ThreadReplyEditModal").modal("hide");
}
function onReplyUpdateRequest_Failure(response: JQueryXHR): void
{
    if (response.statusText === "Thread is locked")
    {
        Alert.raiseAlert("The thread is locked for editing.", "Thread is locked");
    }
    else
    {
        Alert.raiseAlert("An unexpected error occurred while updating");
    }
}

function onThreadEditLinkClick(e: JQueryEventObject): void
{
    $.ajax({
        url: e.data.url,
        type: "GET",
        beforeSend: Forums.onAjaxRequestBegin,
        complete: Forums.onAjaxRequestComplete,
        success: onThreadEditLinkClick_Success,
        error: onThreadEditLinkClick_Failure,
    });
}
function onThreadEditLinkClick_Success(response: string, status: string, jqXhr: JQueryXHR): void
{
    $("#ThreadEditModal").modal("show").find(".modal-body").html(response);
}
function onThreadEditLinkClick_Failure(response: JQueryXHR): void
{
    if (response.statusText === "Invalid ID")
    {
        Alert.raiseAlert("An invalid thread ID was provided", "Invalid Thread");
    }
    else if (response.statusText === "Thread is locked")
    {
        Alert.raiseAlert("The thread is locked for editing", "Thread is locked");
    }
    else
    {
        Alert.raiseAlert("An unexpected error occurred");
    }
}

function onReplyEditLinkClikc(e: JQueryEventObject): void
{
    var replyId: string = $(e.target).closest(".row[data-thread-reply]").attr("data-thread-reply");
    $.ajax({
        url: e.data.url,
        data: { id: replyId },
        type: "GET",
        beforeSend: Forums.onAjaxRequestBegin,
        complete: Forums.onAjaxRequestComplete,
    });
}
function onReplyEditLinkClick_Success(response: string, status: string, jqXhr: JQueryXHR): void
{
    $("#ThreadReplyEditModal").modal("show").find(".modal-body").html(response);
}
function onReplyEditLinkClick_Failure(response: JQueryXHR): void
{
    if (response.statusText === "Invalid ID")
    {
        Alert.raiseAlert("An invalid thread reply ID was provided", "Invalid thread reply");
    }
    else if (response.statusText === "Thread is locked")
    {
        Alert.raiseAlert("The thread is locked for editing", "Thread is locked");
    }
    else
    {
        Alert.raiseAlert("An unexpected error occurred");
    }
}