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

    loadGuildNewsFeedPanel: function Selama_RootArea_Home_LoadGuildNewsFeedPanel(e)
    {
        /// <param name="e" type="jQuery.Event" />
        var $guildNewsPanel = $("#GuildNewsPanel");
        var $loadMoreBtn = $guildNewsPanel.find(".row-load-more");
        var curPage = $guildNewsPanel.attr("data-page") - 0;

        if (isNaN(curPage))
        {
            this._disableGuildNewsFeedLoad($guildNewsPanel);
            return;
        }

        $.ajax()
    },

    
};