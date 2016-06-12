var Selama = Selama || {};
Selama.createElem = Selama.createElem || function __Selama_CreateElem(tagName, cssClassStr, id)
{
    /// <returns type="jQuery" />
    return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
}