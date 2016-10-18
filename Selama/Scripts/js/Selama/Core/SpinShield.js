var Selama = Selama || {};
Selama.Core = Selama.Core || {};
Selama.Core.SpinShield = Selama.Core.SpinShield || {
    _spinShieldSelector: "> .spin-wrapper",
    _defaultTargetSelector: "body",

    raiseShield: function Selama_Core_SpinShield_RaiseShield(target)
    {
        /// <var type="jQuery" />
        var $target = this._getShieldTarget(target);

        if (!this._isValidTarget($target))
        {
            $target = $(this._defaultTargetSelector);
        }

        this._createNewShieldInTarget($target);
        return this;
    },

    _getShieldTarget: function Selama_Core_SpinShield_GetShieldTarget(target)
    {
        if (target instanceof jQuery)
        {
            return target;
        }
        else if (typeof target === typeof '')
        {
            return $(target);
        }
        else
        {
            return $(this._defaultTargetSelector);
        }
    },

    _isValidTarget: function Selama_Core_SpinShield_IsValidTarget($target)
    {
        /// <param name="$target" type="jQuery" />
        return $target.length !== 0;
    },

    _createNewShieldInTarget: function Selama_Core_SpinShield_CreateNewShieldIntarget($target)
    {
        /// <param name="$target" type="jQuery" />
        if ($target.find(this._spinShieldSelector).length !== 0)
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
    },

    lowerShield: function Selama_Core_SpinShield_LowerShield(target)
    {
        var $target = this._getShieldTarget(target);
        if (!this._isValidTarget($target))
        {
            $target = $(this._defaultTargetSelector);
        }
        return this;
    },

    _destroyShieldInTarget: function Selama_Core_SpinShield_DestroyShieldIntarget($target)
    {
        /// <param name="$target" type="jQuery" />
        var $shield = $target.find(this._spinShieldSelector);
        if ($shield.length !== 0)
        {
            $shield.remove();
            $target.css("overflow", "");
        }
    }
};