var Selama = Selama || {};
Selama.createElem = Selama.createElem || function Selama_CreateElem(tagName, cssClassStr, id)
{
    /// <returns type="jQuery" />
    return $(document.createElement(tagName)).addClass(cssClassStr).attr("id", id);
};
Selama.generateFixedTable = function Selama_GenerateFixedTable()
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

// #region SpinShield
Selama.SpinShield = Selama.SpinShield || {
    init: function Selama_SpinShield_Init()
    {
        var $shield = $("#SpinShield");
        if ($shield.length === 0)
        {
            $("body").append(
                Selama.createElem("div", "spin-wrapper hidden", "SpinShield").append(
                    Selama.createElem("div", "spin-wrapper-inner").append(
                        Selama.createElem("div", "spin spin-gleam").spin("show")
                    )
                )
            );
        }
        return this;
    },

    raiseShield: function Selama_SpinShield_RaiseSheild()
    {
        $("#SpinShield.hidden").removeClass("hidden");
        return this;
    },

    lowerShield: function Selama_SpinShield_LowerShield()
    {
        $("#SpinShield").addClass("hidden");
        return this;
    },
};
// #endregion

// #region Alert
Selama.Alert = Selama.Alert || {
    init: function Selama_Alert_Init()
    {
        var $modal = $("#AlertModal.modal");
        if ($modal.length === 0)
        {
            var $modalContent = Selama.createElem("div", "modal-content")
                .append(
                    Selama.createElem("div", "modal-header")
                    .append(
                        Selama.createElem("button", "close")
                        .attr("type", "button")
                        .attr("data-dismiss", "modal")
                        .html("&times;")
                    )
                    .append(
                        Selama.createElem("h4", "modal-title")
                    )
                )
                .append(
                    Selama.createElem("div", "modal-body")
                )
                .append(
                    Selama.createElem("div", "modal-footer")
                    .append(
                        Selama.createElem("button", "btn btn-primary")
                        .attr("type", "button")
                        .attr("data-dismiss", "modal")
                        .text("OK")
                    )
                );
                

            $("body").append(
                Selama.createElem("div", "modal fade", "AlertModal")
                .attr("role", "dialog")
                .attr("data-backdrop", "static")
                .attr("data-keyboard", "false")
                .append(
                    Selama.createElem("div", "modal-dialog")
                    .append(
                        $modalContent
                    )
                )
            );

            $modal = $("#AlertModal");
        }
        
        return $modal;
    },
    raiseAlert: function Selama_Alert_RaiseAlert(text, title)
    {
        if (title === undefined)
        {
            title = "Alert";
        }
        var $modal = $("#AlertModal");
        if ($modal.length === 0)
        {
            $modal = this.init();
        }

        $modal.find(".modal-body").text(text);
        $modal.find(".modal-title").text(title);
        $modal.modal("show");
        return $modal;
    },
};
// #endregion

// #region Page load
$(document).ready(function ()
{
    Selama.SpinShield.init().lowerShield();
    Selama.Alert.init();

    Selama.generateFixedTable();
    $(window).on("resize", "", Selama.generateFixedTable);
});
// #endregion