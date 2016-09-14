define("Core/Selama.Core.Main", ["require", "exports", "Core/Selama.Core", "Core/Selama.Core.Alert", "Core/Selama.Core.SpinShield"], function (require, exports, Core, Alert, SpinShield) {
    "use strict";
    function Run(Selama) {
        Alert.raiseAlert("Testing");
        SpinShield.raiseShield(".jumbotron");
        SpinShield.raiseShield();
        $(window).on("resize", "", Core.generateFixedTables);
        $(".spin-wrapper").click(function () {
            SpinShield.lowerShield($(this).parent());
        });
    }
    exports.Run = Run;
});
//# sourceMappingURL=Selama.Core.Main.js.map