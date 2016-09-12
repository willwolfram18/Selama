System.register(["Selama.Core"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Core;
    var _spinShieldSelector, _defaultTargetSelector;
    function raiseShield(target) {
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
        _createNewShieldInTarget($target);
        return this;
    }
    exports_1("raiseShield", raiseShield);
    function _isValidTarget($target) {
        return $target !== undefined && ($target instanceof jQuery) &&
            $target.length !== 0;
    }
    function _createNewShieldInTarget($target) {
        if ($target.find(this._spinShieldSelector).length === 0) {
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
    exports_1("lowerShield", lowerShield);
    function _destoryShieldInTarget($target) {
        /// <param name="$target" type="jQuery" />
        var $shield = $target.find(this._spinShieldSelector);
        if ($shield.length !== 0) {
            $shield.remove();
            $target.css("overflow", "inherit");
        }
    }
    return {
        setters:[
            function (Core_1) {
                Core = Core_1;
            }],
        execute: function() {
            _spinShieldSelector = "> .spin-wrapper";
            _defaultTargetSelector = "body";
        }
    }
});
//# sourceMappingURL=Selama.Core.SpinShield.js.map