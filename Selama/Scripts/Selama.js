var Selama = Selama || {};
Selama.createElem = Selama.createElem || function __Selama_CreateElem(tagName, cssClassStr, id)
{
    /// <returns type="jQuery" />
    return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
}

Selama.SpinShield = Selama.SpinShield || {
    init: function Selama_SpinShield_Init()
    {
        var $shield = $("#SpinShield");
        if ($shield.length === 0)
        {
            $("body").append(
                Selama.createElem("div", "spin-wrapper hidden", "SpinShield").append(
                    Selama.createElem("div", "spin-wrapper-inner").append(
                        Selama.createElem("div", "spin spin-gleam").spin("show")
                    )
                )
            );
        }
        return this;
    },

    raiseShield: function Selama_SpinShield_RaiseSheild()
    {
        $("#SpinShield.hidden").removeClass("hidden");
        return this;
    },

    lowerSheild: function Selama_SpinShield_lowerShield()
    {
        $("#SpinShield").addClass("hidden");
        return this;
    },
};

$(document).ready(function ()
{
    Selama.SpinShield.init().lowerSheild();
});