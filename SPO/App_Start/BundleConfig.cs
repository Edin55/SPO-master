using System.Web;
using System.Web.Optimization;

namespace SPO
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

                     
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/DataTables/jquery.datatables.js",
                      "~/Scripts/DataTables/datatables.bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/popper.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/DataTables/css/datatables.bootstrap4.css",
                      "~/Content/site.css",
                      "~/Content/todoStyle.css"));
           

        }
    }
}
