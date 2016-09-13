/// <amd-module name="Selama.Core" />
//import * as $ from "jquery";

export function $$bind(func: Function, context: any): Function
{
    return func.bind(context);
}

export function createElem(tagName: string, cssClassStr?: string, id?: string): JQuery
{
    return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
}

export function generateFixedTables(): void
{
    // Remove all fixed tables previously generated
    $(".table.table-fixed-col.active").remove();
    let $tablesToTransform = $(".table.table-fixed.col");
    $tablesToTransform.each(_copyTableToFixedTable);
}
function _copyTableToFixedTable(index: number, elem: Element)
{
    let $this = $(elem);
    let $fixedTable = $this.clone().addClass("active").insertBefore($this);
    $fixedTable.find("th:not(:first-child),td:not(:first-child)").remove();

    // copy the row heights of the original table to the new table
    $fixedTable.find("tr").each(function mirrorOriginalTableRowHeights(copyRowIndex, copyRow)
    {
        let $copyRow = $(copyRow);
        let $originalRow = $this.find("tr:eq(" + copyRowIndex.toString() + ")");
        $copyRow.height($originalRow.height());
    });
}

//// #region Page load
//$(document).ready(function ()
//{
//    Selama.Alert.init();

//    Selama.generateFixedTable();
//    $(window).on("resize", "", Selama.generateFixedTable);
//});
//// #endregion