define("Root/Index", ["require", "exports", "jquery", "Core/SpinShield"], function (require, exports, $, SpinShield) {
    "use strict";
    var $WowheadPower;
    function Setup(wowheadPower, newsFeedUrl) {
        $(document).ready(function () {
            $WowheadPower = wowheadPower;
            var $guildNewsPanel = $("#GuildNewsPanel");
            $guildNewsPanel.on("click", "", { url: newsFeedUrl }, loadGuildNewsFeedPanel);
            $guildNewsPanel.trigger("click");
        });
    }
    exports.Setup = Setup;
    function loadGuildNewsFeedPanel(e) {
        var $guildNewsPanel = $("#GuildNewsPanel");
        var curPage = +$guildNewsPanel.attr("data-page");
        $.ajax({
            url: e.data.url,
            type: "GET",
            data: { page: curPage },
            beforeSend: guildNewsFeedAjaxBeforeSend,
            complete: guildNewsFeedAjaxComplete,
            success: function (response) {
                if (response === "") {
                    return;
                }
                if (!isNaN(curPage)) {
                    $guildNewsPanel.attr("data-page", curPage + 1);
                }
                $guildNewsPanel.find(".panel-body table tbody").append(response);
                $WowheadPower.refreshLinks();
                $("[data-toggle='tooltip']").tooltip();
            }
        });
    }
    function guildNewsFeedAjaxBeforeSend() {
        SpinShield.raiseShield("#GuildNewsPanel .panel-body");
    }
    function guildNewsFeedAjaxComplete() {
        SpinShield.lowerShield("#GuildNewsPanel .panel-body");
    }
});
//# sourceMappingURL=Index.js.map