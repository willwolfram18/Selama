/// <amd-module name="Core/Main" />
/// <amd-dependency name="jquery" />
import Core = require("Core/Common");
import Alert = require("Core/Alert");
import SpinShield = require("Core/SpinShield");
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
