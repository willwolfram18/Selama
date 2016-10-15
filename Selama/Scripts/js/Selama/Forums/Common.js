var Selama = Selama || {};
Selama.Forums = Selama.Forums || {
    _onAjaxRequestBegin: function Selama_Forums_OnAjaxRequestBegin()
    {
        Selama.SpinShield.raiseShield();
    },

    _onAjaxRequestComplete: function Selama_Forums_OnAjaxRequestComplete()
    {
        Selama.SpinShield.lowerShield();
    },
};
Selama.Forums.OnAjaxRequestBeginCallback = Selama.Core.$$bind(Selama.Forums._onAjaxRequestBegin, Selama.Forums);
Selama.Forums.OnAjaxRequestCompleteCallback = Selama.Core.$$bind(Selama.Forums._onAjaxRequestComplete, Selama.Forums);