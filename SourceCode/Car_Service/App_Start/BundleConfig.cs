using System.Web.Optimization;

namespace CarService
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string googleFonts = "https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700";
            string[] LayoutJS = new string[]
            {
                "~/Content/assets/js/bluebird.core.js",
                "~/Content/assets/plugins/jquery/jquery-3.3.1.min.js",
                "~/Content/assets/plugins/jquery-ui/jquery-ui.min.js",
                "~/Content/assets/plugins/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/assets/plugins/slimscroll/jquery.slimscroll.js",
                "~/Content/assets/plugins/js-cookie/js.cookie.js",
                "~/Content/assets/plugins/font-awesome/jx/all.min.js",
                "~/Content/assets/js/theme/default.min.js",
                "~/Content/assets/js/lodash.min.js",
                "~/Content/assets/js/apps.min.js",
                "~/Content/assets/js/Menu.js",
                "~/Content/assets/js/Custom.js",
            };
            string[] LayoutCSS = new string[]
            {
                "~/Content/assets/plugins/jquery-ui/jquery-ui.min.css",
                "~/Content/assets/plugins/bootstrap/css/bootstrap.min.css",
                "~/Content/assets/plugins/font-awesome/css/all.min.css",
                "~/Content/assets/plugins/animate/animate.min.css",
                "~/Content/assets/css/default/style.min.css",
                "~/Content/assets/css/default/style-responsive.min.css",
                "~/Content/assets/css/Custom.css",
                "~/Content/Custom-Fonts.css",
                "~/Content/Site.css"
            };
            string[] DataTblJS = new string[]
            {
                "~/Content/assets/plugins/DataTables/media/js/jquery.dataTables.js",
                "~/Content/assets/plugins/DataTables/media/js/dataTables.bootstrap.min.js",
                "~/Content/assets/plugins/DataTables/extensions/Responsive/js/dataTables.responsive.min.js",
                "~/Content/assets/js/demo/table-manage-fixed-header.demo.min.js",
            };
            string[] DataTblCSS = new string[]
            {
                "~/Content/assets/plugins/DataTables/media/css/dataTables.bootstrap.min.css",
                "~/Content/assets/plugins/DataTables/extensions/Responsive/css/responsive.bootstrap.min.css"
            };
            string[] TrxJS = new string[]
            {
                "~/Content/assets/plugins/iziToast/dist/js/iziToast.min.js",
                "~/Content/assets/plugins/iziModal/js/iziModal.js",
                "~/Content/assets/plugins/Parsleyjs/dist/parsley.min.js",
                "~/Content/assets/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js",
                "~/Content/assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js",
                "~/Content/assets/plugins/bootstrap-datepicker/locales/bootstrap-datepicker.ja.min.js",
                "~/Content/assets/plugins/timepicker/jquery.timepicker.min.js",
                "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                "~/Content/assets/plugins/switchery/switchery.min.js",
                "~/Content/assets/js/Classes/common/Message.js",
                "~/Content/assets/js/Classes/common/Formatter.js",
                "~/Content/assets/js/Classes/common/CustomUI.js",
                "~/Content/assets/js/Classes/common/Data.js",
                "~/Scripts/Helper.js",
            };
            string[] TrxCSS = new string[]
            {
                "~/Content/assets/plugins/iziToast/dist/css/iziToast.min.css",
                "~/Content/assets/plugins/iziModal/css/iziModal.css",
                "~/Content/assets/plugins/Parsleyjs/src/parsley.css",
                "~/Content/assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.css",
                "~/Content/assets/plugins/timepicker/jquery.timepicker.min.css",
                "~/Content/assets/plugins/select2/dist/css/select2.css",
                "~/Content/assets/plugins/select2/dist/css/select2-bootstrap4.css",
                "~/Content/assets/plugins/switchery/switchery.min.css",
                "~/Content/assets/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js",
            };
            bundles.Add(new Bundle("~/Dashboard-CSS", googleFonts)
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );

            bundles.Add(new Bundle("~/Dashboard-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                       "~/Areas/MasterMaintenance/Scripts/Dashboard.js"
                    )
            );
            bundles.Add(new Bundle("~/Login-CSS", googleFonts)
                   .Include(LayoutCSS)
                   .Include(TrxCSS)
            );

            
            bundles.Add(new Bundle("~/Login-JS")
                    .Include(LayoutJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/js/Login.js"
                    )
            );

            bundles.Add(new Bundle("~/Home-JS")
                    .Include(LayoutJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Scripts/Home.js"
                    )
            );

            RegisterMasterMaintenaceBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
            RegisterTransactioneBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
        }
        public static void RegisterMasterMaintenaceBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new Bundle("~/WorkerMaster-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );
            bundles.Add(new Bundle("~/WorkerMaster-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/MasterMaintenance/Scripts/WorkerMaster.js"
                    )
            );

            bundles.Add(new Bundle("~/ServiceMaster-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );
            bundles.Add(new Bundle("~/ServiceMaster-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/MasterMaintenance/Scripts/ServiceMaster.js"
                    )
            );
            bundles.Add(new Bundle("~/GeneralMaster-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );
            bundles.Add(new Bundle("~/GeneralMaster-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/MasterMaintenance/Scripts/GeneralMaster.js"
                    )
            );
        }
        public static void RegisterTransactioneBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new Bundle("~/AssignWorker-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );
            bundles.Add(new Bundle("~/AssignWorker-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/Transaction/Scripts/AssignWorker.js"
                    )
            );

            bundles.Add(new Bundle("~/OngoingService-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );
            bundles.Add(new Bundle("~/OngoingService-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/Transaction/Scripts/OngoingService.js"
                    )
            );

            bundles.Add(new Bundle("~/WalkIn-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
            );
            bundles.Add(new Bundle("~/WalkIn-JS")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/Transaction/Scripts/WalkIn.js"
                    )
            );
        }
    }
}
