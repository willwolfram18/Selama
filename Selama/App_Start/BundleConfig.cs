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
                RootJavaScriptFile("jQuery/jquery-{version}.js"),
                RootJavaScriptFile("jQuery/jquery.unobtrusive-ajax.js"),
                RootJavaScriptFile("jQuery/jquery-ui-{version}.js")
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                RootJavaScriptFile("jQuery/jquery.validate.js"),
                RootJavaScriptFile("jQuery/jquery.validate.unobtrusive.js")
            ));

            bundles.Add(new ScriptBundle("~/bundles/MarkdownDeep").Include(
                RootJavaScriptFile("MarkdownDeep/MarkdownDeepLib.min.js"),
                RootJavaScriptFile("MarkdownDeep/MarkdownDeep.js")
            ));

            bundles.Add(new ScriptBundle("~/bundles/requirejs").Include(
                RootJavaScriptFile("require.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Core").Include(
                RootJavaScriptFile("Selama/Core/Common.js"),
                RootJavaScriptFile("Selama/Core/Alert.js"),
                RootJavaScriptFile("Selama/Core/SpinShield.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-RootArea").Include(
                RootJavaScriptFile("Selama/RootArea/Home/Index.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Forums").Include(
                RootJavaScriptFile("Selama/Forums/Common.js"),
                RootJavaScriptFile("Selama/Forums/Threads.js"),
                RootJavaScriptFile("Selama/Forums/Thread.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Root").Include(
                RootJavaScriptFile("Root/Home/Index.js")
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                RootJavaScriptFile("modernizr-*")
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                RootJavaScriptFile("Bootstrap/bootstrap.js"),
                RootJavaScriptFile("Bootstrap/respond.js"))
            );
            bundles.Add(new ScriptBundle("~/bundles/Selama/Forums").Include(
                "~/Areas/Forums/Scripts/js/Forums.js"
            ));


            bundles.Add(new StyleBundle("~/Content/Selama/css").Include(
                      "~/Content/css/Bootstrap/bootstrap.css",
                      "~/Content/css/Bootstrap/bootstrap-theme.css",
                      "~/Content/css/Bootstrap/bootstrap-social.css",
                      "~/Content/css/FontAwesome/font-awesome.css",
                      "~/Content/css/jquery.spin.css",
                      "~/Content/css/site.css"));
            bundles.Add(new StyleBundle("~/Content/MarkdownDeep/css").Include(
                "~/Content/css/MarkdownDeep/mdd_styles.css",
                "~/Content/css/MarkdownDeep/MarkdownDeep.css"
            ));
        }

        private static string RootJavaScriptFile(string nameInScriptsFolder)
        {
            return string.Format("~/Content/js/{0}", nameInScriptsFolder);
        }
    }
}
