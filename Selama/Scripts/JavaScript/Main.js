define("Main", ["require", "exports", "./Selama.Core", "Selama.Core.Alert", "Selama.Core.SpinShield"], function (require, exports, Core, Alert, SpinShield) {
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
//# sourceMappingURL=Main.js.map