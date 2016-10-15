var Selama = Selama || {};
Selama.Forums = Selama.Forums || {};
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