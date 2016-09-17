﻿interface MarkdownDeepOptions
{
    resizebar?: boolean,
    toolbar?: boolean,
    help_location?: string,
    SafeMode?: boolean,
    ExtraMode?: boolean,
    MarkdownInHtml?: boolean,
    AutoHeadingIDs?: boolean,
    UrlBaseLocation?: string,
    UrlRootLocation?: string,
    NewWindowForExternalLinks?: boolean,
    NewWindowForLocalLinks?: boolean,
    HtmlClassFootnotes?: string,
    HtmlClassTitledImages?: string,
    disableShortCutKeys?: boolean,
    disableAutoIndent?: boolean,
    disableTabHandling?: boolean,
}

interface JQuery 
{
    MarkdownDeep(options?: MarkdownDeepOptions): JQuery;
}

declare module "MarkdownDeepEditor" {
    class Editor
    {
        constructor(editorElement: Element, previewElement: Element);
    }
}
