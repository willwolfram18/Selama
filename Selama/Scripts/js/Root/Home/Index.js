define("Root/Home/Index", ["require", "exports", "jquery", "Core/Common", "Core/SpinShield"], function (require, exports, $, Common, SpinShield) {
    "use strict";
    var $WowheadPower;
    function Setup(wowheadPower, newsFeedUrl) {
        $(document).ready(function () {
            $WowheadPower = wowheadPower;
            var $guildNewsPanel = $("#GuildNewsPanel");
            $guildNewsPanel.on("click", ".row-load-more", { url: newsFeedUrl }, loadGuildNewsFeedPanel);
            $guildNewsPanel.find(".row-load-more").trigger("click");
        });
    }
    exports.Setup = Setup;
    function loadGuildNewsFeedPanel(e) {
        var $guildNewsPanel = $("#GuildNewsPanel");
        var $loadMoreBtn = $guildNewsPanel.find(".row-load-more");
        var curPage = +$guildNewsPanel.attr("data-page");
        if (isNaN(curPage)) {
            disableGuildNewsFeedLoad($guildNewsPanel);
            return;
        }
        $.ajax({
            url: e.data.url,
            type: "GET",
            data: { page: curPage },
            beforeSend: guildNewsFeedAjaxBeforeSend,
            complete: guildNewsFeedAjaxComplete,
            success: function (response) {
                if (response === "") {
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
    function disableGuildNewsFeedLoad($guildNewsPanel) {
        var $loadBtn = $guildNewsPanel.find(".row-load-more");
        $loadBtn.removeClass("row-load-more").find("a").remove();
        Common.createElem("div", "col-sm-12 col-md-12 col-lg-12")
            .html("No more news to load").appendTo($loadBtn);
    }
    function guildNewsFeedAjaxBeforeSend() {
        SpinShield.raiseShield("#GuildNewsPanel .panel-body");
    }
    function guildNewsFeedAjaxComplete() {
        SpinShield.lowerShield("#GuildNewsPanel .panel-body");
    }
});
//# sourceMappingURL=Index.js.map