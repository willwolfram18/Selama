/// <amd-module name="Core/Selama.Core.Main" />
/// <amd-dependency name="jquery" />
import Core = require("Core/Selama.Core");
import Alert = require("Core/Selama.Core.Alert");
import SpinShield = require("Core/Selama.Core.SpinShield");
import $ = require("jquery");

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
