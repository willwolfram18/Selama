define("Selama.Core.SpinShield", ["require", "exports", "Selama.Core"], function (require, exports, Core) {
    "use strict";
    var _spinShieldSelector = "> .spin-wrapper";
    var _defaultTargetSelector = "body";
    function raiseShield(target) {
        var $target;
        if (typeof target === "string") {
            $target = $(target);
        }
        else {
            $target = target;
        }
        if (!_isValidTarget($target)) {
            $target = $(_defaultTargetSelector);
        }
        _createNewShieldInTarget($target);
        return this;
    }
    exports.raiseShield = raiseShield;
    function _isValidTarget($target) {
        return $target !== undefined && ($target instanceof jQuery) &&
            $target.length !== 0;
    }
    function _createNewShieldInTarget($target) {
        if ($target.find(_spinShieldSelector).length === 0) {
            $target.append(Core.createElem("div", "spin-wrapper").append(Core.createElem("div", "spin-wrapper-inner").append(Core.createElem("div", "fa fa-4x fa-circle-o-notch fa-spin"))))
                .css("overflow", "hidden");
        }
    }
    function lowerShield(target) {
        var $target;
        if (typeof target === "string") {
            $target = $(target);
        }
        else {
            $target = target;
        }
        if (!_isValidTarget($target)) {
            target = $(_defaultTargetSelector);
        }
        _destoryShieldInTarget($target);
    }
    exports.lowerShield = lowerShield;
    function _destoryShieldInTarget($target) {
        /// <param name="$target" type="jQuery" />
        var $shield = $target.find(_spinShieldSelector);
        if ($shield.length !== 0) {
            $shield.remove();
            $target.css("overflow", "inherit");
        }
    }
});
//# sourceMappingURL=Selama.Core.SpinShield.js.map