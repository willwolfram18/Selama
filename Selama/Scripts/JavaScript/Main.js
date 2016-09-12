System.register(["Selama.Core"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Core;
    return {
        setters:[
            function (Core_1) {
                Core = Core_1;
            }],
        execute: function() {
            $(document).ready(function () {
                Core.generateFixedTables();
            });
        }
    }
});
//# sourceMappingURL=Main.js.map