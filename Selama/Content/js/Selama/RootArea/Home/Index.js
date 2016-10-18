var Selama;
Selama.RootArea = Selama.RootArea || {};
Selama.RootArea.Home = Selama.RootArea.Home || {
    _defaultShieldTarget: "#GuildNewsPanel .panel",

    _disableGuildNewsFeedLoad: function Selama_RootArea_Home_DisableGuildNewsFeedLoad($guildNewsPanel)
    {
        /// <param name="$guildNewsPanel" type="jQuery" />
        var $loadBtn = $guildNewsPanel.find(".row-load-more");
        $loadBtn.removeClass("row-load-more").empty();
        $loadBtn.append(
            Selama.Core.createElem("div", "col-sm-12 col-md-12 col-lg-12")
                .text("No more news to load")
        );
    },

    _guildNewsFeedAjaxBeforeSend: function Selama_RootArea_Home_GuildNewsFeedAjaxBeforeSend()
    {
        Selama.Core.SpinShield.raiseShield(this._defaultShieldTarget);
    },
    
    _guildNewsFeedAjaxComplete: function Selama_RootArea_Home_GuildNewsFeedAjaxComplete()
    {
        Selama.Core.SpinShield.lowerShield(this._defaultShieldTarget);
    },

    _loadGuildNewsFeedPanel: function Selama_RootArea_Home_InternalLoadGuildNewsFeedPanel(e)
    {
        /// <param name="e" type="jQuery.Event" />
        var $guildNewsPanel = $("#GuildNewsPanel");
        var $loadMoreBtn = $guildNewsPanel.find(".row-load-more");
        var curPage = $guildNewsPanel.attr("data-page") - 0;

        if (isNaN(curPage))
        {
            Selama.RootArea.Home._disableGuildNewsFeedLoad($guildNewsPanel);
            return;
        }

        var beforeSendCallback = Selama.Core.$$bind(Selama.RootArea.Home._guildNewsFeedAjaxBeforeSend, Selama.RootArea.Home);
        var completeCallback = Selama.Core.$$bind(Selama.RootArea.Home._guildNewsFeedAjaxComplete, Selama.RootArea.Home);
        var successCallback = Selama.Core.$$bind(function _guildNewsFeedAjaxSuccess(response)
        {
            if (response === "")
            {
                this._disableGuildNewsFeedLoad($guildNewsPanel);
                return;
            }

            $guildNewsPanel.attr("data-page", curPage + 1);
            $(response).insertBefore($loadMoreBtn);
            $WowheadPower.refreshLinks();
            $("[data-toggle='tooltip']").tooltip();
        }, Selama.RootArea.Home);

        $.ajax({
            url: e.data.url,
            type: "GET",
            data: { page: curPage },
            beforeSend: beforeSendCallback,
            complete: completeCallback,
            success: successCallback,
        });
    },
};
Selama.RootArea.Home.LoadGuildNewsFeedPanelCallback = Selama.Core.$$bind(Selama.RootArea.Home._loadGuildNewsFeedPanel, Selama.RootArea.Home);