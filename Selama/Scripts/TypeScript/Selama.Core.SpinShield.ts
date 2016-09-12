/// <reference path="Selama.Core.ts" />
export namespace Selama.Core.SpinShield
{
    let _spinShieldSelector = "> .spin-wrapper";
    let _defaultTargetSelector = "body";

    export function raiseShield(target: JQuery | string)
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
            target = $(_defaultTargetSelector);
        }
        _createNewShieldInTarget($target);
        return this;
    }

    function _isValidTarget($target: JQuery): boolean
    {
        return $target !== undefined && ($target instanceof jQuery) &&
            $target.length !== 0;
    }

    function _createNewShieldInTarget($target: JQuery)
    {
        if ($target.find(this._spinShieldSelector).length === 0)
        {
            $target.append(
                Selama.Core.createElem("div", "spin-wrapper").append(
                    Selama.Core.createElem("div", "spin-wrapper-inner").append(
                        Selama.Core.createElem("div", "fa fa-4x fa-circle-o-notch fa-spin")
                    )
                )
            )
                .css("overflow", "hidden");
        }
    }

    export function lowerShield(target: JQuery | string)
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
            target = $(_defaultTargetSelector);
        }
        _destoryShieldInTarget($target);
    }

    function _destoryShieldInTarget($target: JQuery)
    {
        /// <param name="$target" type="jQuery" />
        var $shield = $target.find(this._spinShieldSelector);
        if ($shield.length !== 0)
        {
            $shield.remove();
            $target.css("overflow", "inherit");
        }
    }
}