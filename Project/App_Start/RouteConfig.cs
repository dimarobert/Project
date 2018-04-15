using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace Project {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("enum", typeof(EnumConstraint));
            routes.MapMvcAttributeRoutes(constraintResolver);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    class EnumConstraint : IRouteConstraint {
        private static readonly ConcurrentDictionary<string, string[]> Cache = new ConcurrentDictionary<string, string[]>();
        private readonly string[] _validOptions;
        /// <summary>
        /// Create new <see cref="EnumConstraint"/>
        /// </summary>
        /// <param name="enumType"></param>
        public EnumConstraint(string enumType) {
            _validOptions = Cache.GetOrAdd(enumType, key => {
                var type = Type.GetType(key);
                return type != null ? Enum.GetNames(type) : new string[0];
            });
        }
        /// <inheritdoc />
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) {
            object value;
            if (values.TryGetValue(parameterName, out value) && value != null) {
                return _validOptions.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
