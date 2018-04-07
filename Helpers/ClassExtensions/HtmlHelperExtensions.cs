using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.Helpers.ClassExtensions {

    public static class HtmlHelperExtensions {
        public static string IsSelected(this HtmlHelper<dynamic> html, string controllers = "", string actions = "", string cssClass = "selected") {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(act => act.Trim()).Distinct().ToArray();
            string[] acceptedControllers = controllers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(act => act.Trim()).Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }

    }
}