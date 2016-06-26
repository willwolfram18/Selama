using System.Web;
using System.Web.Optimization;

namespace Selama
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery/jquery-{version}.js",
                        "~/Scripts/jQuery/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jQuery/jquery-ui-{version}.js",
                        "~/Scripts/jQuery/jquery.spin.js",
                        "~/Scripts/Selama.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jQuery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/MarkdownDeep").Include(
                "~/Scripts/MarkdownDeep/MarkdownDeepLib.min.js",
                "~/Scripts/MarkdownDeep/MarkdownDeep.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Bootstrap/bootstrap.js",
                      "~/Scripts/Bootstrap/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Bootstrap/bootstrap.css",
                      "~/Content/Bootstrap/bootstrap-theme.css",
                      "~/Content/FontAwesome/font-awesome.css",
                      "~/Content/jquery.spin.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/MarkdownDeep/css").Include(
                "~/Content/MarkdownDeep/mdd_styles.css",
                "~/Content/MarkdownDeep/MarkdownDeep.css"
            ));
        }
    }
}
