System.register(["jquery"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var $;
    function $$bind(func, context) {
        return func.bind(context);
    }
    exports_1("$$bind", $$bind);
    function createElem(tagName, cssClassStr, id) {
        return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
    }
    exports_1("createElem", createElem);
    function generateFixedTables() {
        // Remove all fixed tables previously generated
        $(".table.table-fixed-col.active").remove();
        var $tablesToTransform = $(".table.table-fixed.col");
        $tablesToTransform.each(_copyTableToFixedTable);
    }
    exports_1("generateFixedTables", generateFixedTables);
    function _copyTableToFixedTable(index, elem) {
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
    return {
        setters:[
            function ($_1) {
                $ = $_1;
            }],
        execute: function() {
        }
    }
});
//// #region Page load
//$(document).ready(function ()
//{
//    Selama.Alert.init();
//    Selama.generateFixedTable();
//    $(window).on("resize", "", Selama.generateFixedTable);
//});
//// #endregion 
//# sourceMappingURL=Selama.Core.js.map