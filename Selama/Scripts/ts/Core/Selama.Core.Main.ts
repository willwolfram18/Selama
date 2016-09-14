/// <amd-module name="Core/Selama.Core.Main" />
import * as Core from "Core/Selama.Core";
import * as Alert from "Core/Selama.Core.Alert";
import * as SpinShield from "Core/Selama.Core.SpinShield";

export function Run(Selama: Object)
{
    Alert.raiseAlert("Testing");
    SpinShield.raiseShield(".jumbotron");
    SpinShield.raiseShield();
    $(window).on("resize", "", Core.generateFixedTables);

    $(".spin-wrapper").click(function ()
    {
        SpinShield.lowerShield($(this).parent());
    });
}
