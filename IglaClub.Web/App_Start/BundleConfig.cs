using System.Web;
using System.Web.Optimization;

namespace IglaClub.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/qtip").Include(
                        "~/Content/qtip/jquery.qtip.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/DateTimepickerJs").Include
                ("~/Scripts/jquery.datetimepicker.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            "~/Content/bootstrap/js/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/searchable").Include(
            "~/Scripts/jquery.searchabledropdown-1.0.8.src.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap/css/bootstrap.css",
                        "~/Content/bootstrap/css/bootstrap-responsive.css",
                        "~/Content/global.css",
                        "~/Content/dataTables/css/jquery.dataTables.css",
                        "~/Content/dataTables/css/jquery.dataTables_themeroller.css",
                        "~/Content/qtip/jquery.qtip.css"
                        ));
            
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
            "~/Content/dataTables/js/jquery.dataTables.js"));

          
            bundles.Add(new StyleBundle("~/Content/DateTimepickerCss").Include
                ("~/Content/jquery.datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include
                ("~/Content/jquery-ui.css"));

        }
    }
}