/// <amd-module name="Forums/Common" />
import SpinShield = require("Core/SpinShield");

export function onAjaxRequestBegin(): void
{
    console.log("onAjaxRequestBegin");
    SpinShield.raiseShield();
}

export function onAjaxRequestEnd(): void
{
    console.log("onAjaxRequestEnd");
    SpinShield.lowerShield();
}