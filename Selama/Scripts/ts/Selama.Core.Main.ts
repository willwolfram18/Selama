/// <amd-module name="Selama.Core.Main" />
import Core = require("Selama.Core");
import Alert = require("Selama.Core.Alert");
import SpinShield = require("Selama.Core.SpinShield");

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
