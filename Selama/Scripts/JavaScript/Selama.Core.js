/// <amd-module name="Selama.Core" />
define("Selama.Core", ["require", "exports"], function (require, exports) {
    "use strict";
    function $$bind(func, context) {
        return func.bind(context);
    }
    exports.$$bind = $$bind;
    function createElem(tagName, cssClassStr, id) {
        return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
    }
    exports.createElem = createElem;
    function generateFixedTables() {
        // Remove all fixed tables previously generated
        $(".table.table-fixed-col.active").remove();
        var $tablesToTransform = $(".table.table-fixed.col");
        $tablesToTransform.each(_copyTableToFixedTable);
    }
    exports.generateFixedTables = generateFixedTables;
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
});
//# sourceMappingURL=Selama.Core.js.map