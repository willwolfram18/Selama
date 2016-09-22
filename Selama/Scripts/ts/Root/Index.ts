/// <amd-module name="Root/Index" />
import $ = require("jquery");
import SpinShield = require("Core/SpinShield");

var $WowheadPower;

export function Setup(wowheadPower: any, newsFeedUrl: string): void
{
    $(document).ready(() =>
    {
        $WowheadPower = wowheadPower;
        let $guildNewsPanel: JQuery = $("#GuildNewsPanel");
        $guildNewsPanel.on("click", "", { url: newsFeedUrl }, loadGuildNewsFeedPanel);
    });
}

function loadGuildNewsFeedPanel(e: JQueryEventObject): void
{
    let $guildNewsPanel: JQuery = $("#GuildNewsPanel");
    let curPage: number = +$guildNewsPanel.attr("data-page");

    $.ajax({
        url: e.data.url,
        type: "GET",
        data: { page: curPage },
        beforeSend: guildNewsFeedAjaxBeforeSend,
        complete: guildNewsFeedAjaxComplete,
        success: (response: string) =>
        {
            if (!isNaN(curPage))
            {
                $guildNewsPanel.attr("data-page", curPage + 1);
            }
            $guildNewsPanel.find(".panel-body table tbody").append(response);
            $WowheadPower.refreshLinks();
            $("[data-toggle='tooltip']").tooltip();
        }
    });
}

function guildNewsFeedAjaxBeforeSend(): void
{
    SpinShield.raiseShield("#GuildNewsPanel .panel-body");
}

function guildNewsFeedAjaxComplete(): void
{
    SpinShield.lowerShield("#GuildNewsPanel .panel-body");
}