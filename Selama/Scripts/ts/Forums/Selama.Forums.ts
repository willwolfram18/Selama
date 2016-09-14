/// <amd-module name="Forums/Selama.Forums" />
import * as SpinShield from "Core/Selama.Core.SpinShield";

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