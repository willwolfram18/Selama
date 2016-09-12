"use strict";
var $ = require("jquery");
var Selama;
(function (Selama) {
    var Core;
    (function (Core) {
        function $$bind(func, context) {
            return func.bind(context);
        }
        Core.$$bind = $$bind;
        function createElem(tagName, cssClassStr, id) {
            return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
        }
        Core.createElem = createElem;
        function generateFixedTables() {
            // Remove all fixed tables previously generated
            $(".table.table-fixed-col.active").remove();
            var $tablesToTransform = $(".table.table-fixed.col");
            $tablesToTransform.each(copyTableToFixedTable);
        }
        Core.generateFixedTables = generateFixedTables;
        function copyTableToFixedTable(index, elem) {
            var $this = $(elem);
            var $fixedTable = $this.clone().addClass("active").insertBefore($this);
            $fixedTable.find("th:not(:first-child),td:not(:first-child)").remove();
            // copy the row heights of the original table to the new table
            $fixedTable.find("tr").each(function mirrorOriginalTableRowHeights(copyRowIndex, copyRow) {
                var $copyRow = $(copyRow);
                var $originalRow = $this.find("tr:eq(" + copyRowIndex.toString() + ")");
                $copyRow.height($originalRow.height());
            });
        }
    })(Core = Selama.Core || (Selama.Core = {}));
})(Selama = exports.Selama || (exports.Selama = {}));
//// #region SpinShield
//Selama.SpinShield = Selama.SpinShield || {
//    _spinShieldSelector: "> .spin-wrapper",
//    _defaultTargetSelector: "body",
//    raiseShield: function Selama_SpinShield_RaiseSheild($target)
//    {
//        /// <param name="$target" type="jQuery" />
//        if (!this._isValidTarget($target))
//        {
//            $target = $(this._defaultTargetSelector);
//        }
//        this._createNewShieldInTarget($target);
//        return this;
//    },
//    _isValidTarget: function Selama_SpinShield_IsValidTarget($target)
//    {
//        /// <param name="$target" type="jQuery" />
//        /// <returns type="Boolean" />
//        return $target !== undefined && ($target instanceof jQuery) &&
//            $target.length !== 0;
//    },
//    _createNewShieldInTarget: function Selama_SpinShield_CreateNewShieldInTarget($target)
//    {
//        /// <param name="$target" type="jQuery" />
//        if ($target.find(this._spinShieldSelector).length === 0)
//        {
//            $target.append(
//                Selama.createElem("div", "spin-wrapper").append(
//                    Selama.createElem("div", "spin-wrapper-inner").append(
//                        Selama.createElem("div", "fa fa-4x fa-circle-o-notch fa-spin")
//                    )
//                )
//            )
//                .css("overflow", "hidden");
//        }
//        return $target.find(this._spinShieldSelector);
//    },
//    lowerShield: function Selama_SpinShield_LowerShield($target)
//    {
//        if (!this._isValidTarget($target))
//        {
//            $target = $(this._defaultTargetSelector);
//        }
//        this._destoryShieldInTarget($target);
//        return this;
//    },
//    _destoryShieldInTarget: function Selama_SpinShield_DestroyShieldInTarget($target)
//    {
//        /// <param name="$target" type="jQuery" />
//        var $shield = $target.find(this._spinShieldSelector);
//        if ($shield.length !== 0)
//        {
//            $shield.remove();
//            $target.css("overflow", "inherit");
//        }
//    },
//};
//// #endregion
//// #region Alert
//Selama.Alert = Selama.Alert || {
//    init: function Selama_Alert_Init()
//    {
//        var $modal = $("#AlertModal.modal");
//        if ($modal.length === 0)
//        {
//            var $modalContent = Selama.createElem("div", "modal-content")
//                .append(
//                Selama.createElem("div", "modal-header")
//                    .append(
//                    Selama.createElem("button", "close")
//                        .attr("type", "button")
//                        .attr("data-dismiss", "modal")
//                        .html("&times;")
//                    )
//                    .append(
//                    Selama.createElem("h4", "modal-title")
//                    )
//                )
//                .append(
//                Selama.createElem("div", "modal-body")
//                )
//                .append(
//                Selama.createElem("div", "modal-footer")
//                    .append(
//                    Selama.createElem("button", "btn btn-primary")
//                        .attr("type", "button")
//                        .attr("data-dismiss", "modal")
//                        .text("OK")
//                    )
//                );
//            $("body").append(
//                Selama.createElem("div", "modal fade", "AlertModal")
//                    .attr("role", "dialog")
//                    .attr("data-backdrop", "static")
//                    .attr("data-keyboard", "false")
//                    .append(
//                    Selama.createElem("div", "modal-dialog")
//                        .append(
//                        $modalContent
//                        )
//                    )
//            );
//            $modal = $("#AlertModal");
//        }
//        return $modal;
//    },
//    raiseAlert: function Selama_Alert_RaiseAlert(text, title)
//    {
//        if (title === undefined)
//        {
//            title = "Alert";
//        }
//        var $modal = $("#AlertModal");
//        if ($modal.length === 0)
//        {
//            $modal = this.init();
//        }
//        $modal.find(".modal-body").text(text);
//        $modal.find(".modal-title").text(title);
//        $modal.modal("show");
//        return $modal;
//    },
//};
//// #endregion
//// #region Page load
//$(document).ready(function ()
//{
//    Selama.Alert.init();
//    Selama.generateFixedTable();
//    $(window).on("resize", "", Selama.generateFixedTable);
//});
//// #endregion 
//# sourceMappingURL=Selama.Core.js.map