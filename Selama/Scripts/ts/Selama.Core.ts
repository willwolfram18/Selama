/// <amd-module name="Selama.Core" />

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
    let $tablesToTransform: JQuery = $(".table.table-fixed.col");
    $tablesToTransform.each(_copyTableToFixedTable);
}
function _copyTableToFixedTable(index: number, elem: Element): void
{
    let $this: JQuery = $(elem);
    let $fixedTable: JQuery = $this.clone().addClass("active").insertBefore($this);
    $fixedTable.find("th:not(:first-child),td:not(:first-child)").remove();

    // copy the row heights of the original table to the new table
    $fixedTable.find("tr").each(function mirrorOriginalTableRowHeights(copyRowIndex: number, copyRow: Element): any
    {
        let $copyRow: JQuery = $(copyRow);
        let $originalRow: JQuery = $this.find("tr:eq(" + copyRowIndex.toString() + ")");
        $copyRow.height($originalRow.height());
    });
}