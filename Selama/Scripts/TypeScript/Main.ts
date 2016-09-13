﻿/// <amd-module name="Main" />
import * as Core from "./Selama.Core";
import * as Alert from "./Selama.Core.Alert";
import * as SpinShield from "./Selama.Core.SpinShield";

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
