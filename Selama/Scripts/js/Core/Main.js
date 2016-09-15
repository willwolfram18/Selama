define("Core/Main", ["require", "exports", "Core/Common", "Core/Alert", "Core/SpinShield", "jquery"], function (require, exports, Core, Alert, SpinShield, $) {
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