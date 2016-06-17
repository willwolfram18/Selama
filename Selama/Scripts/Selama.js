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

        }
    }
};