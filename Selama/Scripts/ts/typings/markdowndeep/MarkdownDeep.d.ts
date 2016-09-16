/// <reference path="../jquery/jquery.d.ts" />

interface MarkdownDeepOptions
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

interface Editor
{
    constructor(editorElement: Element, previewElement: Element);
}

declare module "MarkdownDeep"{}