var Selama = Selama || {};

Selama.Core = {};
Selama.Core.$$bind = function Selama_Core_$$bind(func, context)
{
    /// <param name="func" type="Function" />
    /// <param name="context" type="Object" />
    return func.bind(context);
};

Selama.Core.createElem = function Selama_Core_CreateElement(tagName, cssClass, id)
{
    /// <param name="tagName" type="String" />
    /// <param name="cssClass" type="String" />
    /// <param name="id" type="String" />
    /// <returns type="jQuery" />
    return $("<" + tagName + "/>").addClass(cssClass).attr("id", id);
};

Selama.Core.generateFixedTable = function Selama_Core_GenerateFixedTable()
{
    $(".table.table-fixed-col.active").remove();
    var $table = $(".table.table-fixed-col");
    var $fixedTable = $table.clone().insertBefore($(".table")).addClass("active");
    $fixedTable.find("th:not(:first-child),td:not(:first-child)").remove();
    $fixedTable.find("tr").each(function (i, elem)
    {
        $(this).height($table.find("tr:eq(" + i + ")").height());
    });
};