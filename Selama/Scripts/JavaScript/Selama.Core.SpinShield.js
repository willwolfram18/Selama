"use strict";
/// <reference path="Selama.Core.ts" />
var Selama_Core_1 = require("./Selama.Core");
var Selama;
(function (Selama) {
    var Core;
    (function (Core) {
        var SpinSheild;
        (function (SpinSheild) {
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
                    target = $(_defaultTargetSelector);
                }
                _createNewShieldInTarget($target);
                return this;
            }
            SpinSheild.raiseShield = raiseShield;
            function _isValidTarget($target) {
                return $target !== undefined && ($target instanceof jQuery) &&
                    $target.length !== 0;
            }
            function _createNewShieldInTarget($target) {
                if ($target.find(this._spinShieldSelector).length === 0) {
                    $target.append(Selama_Core_1.default.createElem("div", "spin-wrapper").append(Selama_Core_1.default.createElem("div", "spin-wrapper-inner").append(Selama_Core_1.default.createElem("div", "fa fa-4x fa-circle-o-notch fa-spin"))))
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
            SpinSheild.lowerShield = lowerShield;
            function _destoryShieldInTarget($target) {
                /// <param name="$target" type="jQuery" />
                var $shield = $target.find(this._spinShieldSelector);
                if ($shield.length !== 0) {
                    $shield.remove();
                    $target.css("overflow", "inherit");
                }
            }
        })(SpinSheild = Core.SpinSheild || (Core.SpinSheild = {}));
    })(Core = Selama.Core || (Selama.Core = {}));
})(Selama = exports.Selama || (exports.Selama = {}));
//# sourceMappingURL=Selama.Core.SpinShield.js.map