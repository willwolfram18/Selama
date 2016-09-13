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
                        "~/Scripts/JavaScript/jQuery/jquery-{version}.js",
                        "~/Scripts/JavaScript/jQuery/jquery.unobtrusive-ajax.js",
                        "~/Scripts/JavaScript/jQuery/jquery-ui-{version}.js",
                        "~/Scripts/JavaScript/jQuery/jquery.spin.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/JavaScript/jQuery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/MarkdownDeep").Include(
                "~/Scripts/JavaScript/MarkdownDeep/MarkdownDeepLib.min.js",
                "~/Scripts/JavaScript/MarkdownDeep/MarkdownDeep.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/requirejs").Include(
                "~/Scripts/JavaScript/require.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Core").Include(
                "~/Scripts/JavaScript/Selama.Core.js",
                "~/Scripts/JavaScript/Selama.Core.Alert.js",
                "~/Scripts/JavaScript/Selama.Core.SpinShield.js",
                "~/Scripts/JavaScript/Main.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/JavaScript/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/JavaScript/Bootstrap/bootstrap.js",
                      "~/Scripts/JavaScript/Bootstrap/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/Selama/Forums").Include(
                "~/Areas/Forums/Scripts/JavaScript/Forums.js"
            ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Bootstrap/bootstrap.css",
                      "~/Content/Bootstrap/bootstrap-theme.css",
                      "~/Content/Bootstrap/bootstrap-social.css",
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
