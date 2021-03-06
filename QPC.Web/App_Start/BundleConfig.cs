﻿using System.Web;
using System.Web.Optimization;

namespace QPC.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/Helpers/ErrorHelper.js",
                "~/Scripts/app/Instruction/InstructionService.js",
                "~/Scripts/app/Instruction/InstructionController.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                      "~/Scripts/materialize/materialize.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     //"~/Content/materialize-css/materialize.min.css",
                     "~/Content/Icons.css",
                      "~/Content/site.css"));
            
        }
    }
}
