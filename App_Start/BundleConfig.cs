using System;
using System.Web;
using System.Web.Optimization;

namespace MobileReimbursement
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            /*
             * Created By Jayendrasinh Gohil
             * Date: 26/12/2017
             */
            ScriptBundle scriptBndl = new ScriptBundle("~/bundles/AllScripts");


            //use Include() method to add all the script files with their paths 
            scriptBndl.Include(
                                  "~/Scripts/bootstrap.min.js",
        "~/Scripts/jquery.easypiechart.min.js",
        "~/Scripts/chosen.jquery.min.js",
        "~/Scripts/jquery.sparkline.min.js",
        "~/Scripts/jquery.flot.min.js",
        "~/Scripts/jquery.flot.pie.min.js",
        "~/Scripts/jquery.flot.resize.min.js",
        "~/Scripts/ace-elements.min.js",
        "~/Scripts/ace.min.js",
        "~/Scripts/jquery.validate.min.js",
        "~/Scripts/jquery.form.js",
        "~/Scripts/json2.js",
        "~/Scripts/jquery.gritter.min.js",
        "~/Scripts/select2.min.js",
        "~/Scripts/bootstrap-datepicker.min.js",
        "~/Scripts/jquery.jqGrid.min.js",
        "~/Scripts/grid.locale-en.js",
        "~/Scripts/bootstrap-timepicker.min.js",
        "~/Scripts/typeahead.jquery.min.js",
        "~/Scripts/jquery.inputlimiter.min.js",
        "~/Scripts/MyScripts.js",
        "~/Scripts/RFCCommonScripts.js"
                              );


            //Add the bundle into BundleCollection
            bundles.Add(scriptBndl);

            BundleTable.EnableOptimizations = true;

        }
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}