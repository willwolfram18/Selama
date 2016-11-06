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

            bundles.Add(new ScriptBundle("~/bundles/Selama-Core").Include(
                SelamaJavaScriptFile("Core/Common.js"),
                SelamaJavaScriptFile("Core/Alert.js"),
                SelamaJavaScriptFile("Core/SpinShield.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-RootArea").Include(
                SelamaJavaScriptFile("RootArea/Home/Index.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Forums").Include(
                SelamaJavaScriptFile("Forums/Common.js"),
                SelamaJavaScriptFile("Forums/Threads.js"),
                SelamaJavaScriptFile("Forums/Thread.js")
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


            bundles.Add(new StyleBundle("~/Content/bundles/Selama/css").Include(
                RootCssFile("Bootstrap/bootstrap.css"),
                RootCssFile("Bootstrap/bootstrap-theme.css"),
                RootCssFile("Bootstrap/bootstrap-social.css"),
                RootCssFile("FontAwesome/font-awesome.css"),
                RootCssFile("jquery.spin.css"),
                RootCssFile("site.css")
            ));
            bundles.Add(new StyleBundle("~/Content/MarkdownDeep/css").Include(
                RootCssFile("MarkdownDeep/mdd_styles.css"),
                RootCssFile("MarkdownDeep/MarkdownDeep.css")
            ));
        }

        private static string RootJavaScriptFile(string nameInJsFolder)
        {
            return string.Format("~/Content/js/{0}", nameInJsFolder);
        }
        private static string RootCssFile(string nameInCssFolder)
        {
            return string.Format("~/Content/css/{0}", nameInCssFolder);
        }
        private static string SelamaJavaScriptFile(string jsFileName)
        {
            return RootJavaScriptFile(string.Format("Selama/{0}", jsFileName));
        }
    }
}
