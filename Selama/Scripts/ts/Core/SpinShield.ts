/// <amd-module name="Core/SpinShield" />
/// <amd-dependency name="jquery" />
import Core = require("Core/Common");
import $ = require("jquery");

let _spinShieldSelector = "> .spin-wrapper";
let _defaultTargetSelector = "body";

export function raiseShield(target?: JQuery | string): void
{
    let $target: JQuery;
    if (typeof target === "string")
    {
        $target = $(target);
    }
    else
    {
        $target = <JQuery>target;
    }
    if (!_isValidTarget($target))
    {
        $target = $(_defaultTargetSelector);
    }
    _createNewShieldInTarget($target);
    return this;
}

function _isValidTarget($target: JQuery): boolean
{
    return $target !== undefined && ($target instanceof jQuery) &&
        $target.length !== 0;
}

function _createNewShieldInTarget($target: JQuery): void
{
    if ($target.find(_spinShieldSelector).length === 0)
    {
        $target.append(
            Core.createElem("div", "spin-wrapper").append(
                Core.createElem("div", "spin-wrapper-inner").append(
                    Core.createElem("div", "fa fa-4x fa-circle-o-notch fa-spin")
                )
            )
        )
        .css("overflow", "hidden");
    }
}

export function lowerShield(target?: JQuery | string): void
{
    let $target: JQuery;
    if (typeof target === "string")
    {
        $target = $(target);
    }
    else
    {
        $target = <JQuery>target;
    }
    if (!_isValidTarget($target))
    {
        $target = $(_defaultTargetSelector);
    }
    _destoryShieldInTarget($target);
}

function _destoryShieldInTarget($target: JQuery): void
{
    /// <param name="$target" type="jQuery" />
    var $shield: JQuery = $target.find(_spinShieldSelector);
    if ($shield.length !== 0)
    {
        $shield.remove();
        $target.css("overflow", "");
    }
}
