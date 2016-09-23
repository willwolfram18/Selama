/// <amd-module name="Root/Home/Index" />
import $ = require("jquery");
import SpinShield = require("Core/SpinShield");

var $WowheadPower;

export function Setup(wowheadPower: any, newsFeedUrl: string): void
{
    $(document).ready(() =>
    {
        $WowheadPower = wowheadPower;
        let $guildNewsPanel: JQuery = $("#GuildNewsPanel");
        $guildNewsPanel.on("click", ".row-load-more", { url: newsFeedUrl }, loadGuildNewsFeedPanel);
        $guildNewsPanel.find(".row-load-more").trigger("click");
    });
}

function loadGuildNewsFeedPanel(e: JQueryEventObject): void
{
    let $guildNewsPanel: JQuery = $("#GuildNewsPanel");
    let $loadMoreBtn: JQuery = $guildNewsPanel.find(".row-load-more");
    let curPage: number = +$guildNewsPanel.attr("data-page");

    if (isNaN(curPage))
    {
        disableGuildNewsFeedLoad($guildNewsPanel);
        return;
    }

    $.ajax({
        url: e.data.url,
        type: "GET",
        data: { page: curPage },
        beforeSend: guildNewsFeedAjaxBeforeSend,
        complete: guildNewsFeedAjaxComplete,
        success: (response: string) =>
        {
            if (response === "")
            {
                disableGuildNewsFeedLoad($guildNewsPanel);
                return;
            }

            // Setup for next load call
            $guildNewsPanel.attr("data-page", curPage + 1);
            // Insert and update UI
            $(response).insertBefore($loadMoreBtn);
            $WowheadPower.refreshLinks();
            $("[data-toggle='tooltip']").tooltip();
        }
    });
}

function disableGuildNewsFeedLoad($guildNewsPanel: JQuery): void
{
    $guildNewsPanel.find(".row-load-more").removeClass("row-load-more").find("td").text("No more news to load");
}

function guildNewsFeedAjaxBeforeSend(): void
{
    SpinShield.raiseShield("#GuildNewsPanel .panel-body");
}

function guildNewsFeedAjaxComplete(): void
{
    SpinShield.lowerShield("#GuildNewsPanel .panel-body");
}