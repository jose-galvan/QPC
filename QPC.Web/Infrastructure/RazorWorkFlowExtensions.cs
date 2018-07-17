using System.Web.Mvc;
using System.Web.Routing;

namespace QPC.Web.Infrastructure
{
    public static class RazorWorkFlowExtensions
    {

        public static MvcHtmlString WorkFlowActionLink(this HtmlHelper html, string action, string controller,
            string displayText, int linkTargetStage, int currentStage, int minRequiredStage, int highestCompletedStage)
        {
            if (highestCompletedStage >= minRequiredStage)
            {
                //Generate URL
                var targetUrl = UrlHelper.GenerateUrl("Default", action, controller,
                    null, RouteTable.Routes, html.ViewContext.RequestContext, false);
                //Create <a> tag
                var anchorBuilder = new TagBuilder("a");
                anchorBuilder.MergeAttribute("href", targetUrl);

                //Assign CSS classes
                string classes = "btn btn-progress";
                if (linkTargetStage == currentStage)
                {
                    classes += " btn-progress-active";
                }
                anchorBuilder.MergeAttribute("class", classes);

                //return as MVC string 
                anchorBuilder.InnerHtml = displayText;
                return new MvcHtmlString(anchorBuilder.ToString(TagRenderMode.Normal));
            }
            else
            {
                var spanBuilder = new TagBuilder("span");
                spanBuilder.MergeAttribute("class", "btn btn-progress");
                spanBuilder.InnerHtml = displayText;
                return new MvcHtmlString(spanBuilder.ToString(TagRenderMode.Normal));
            }
        }
    }
}