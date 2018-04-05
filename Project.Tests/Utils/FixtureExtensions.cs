using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project.Tests.Utils {

    internal class WebModelCustomization : CompositeCustomization {
        internal WebModelCustomization()
            : base(
                new MvcCustomization(),
                new AutoMoqCustomization() { ConfigureMembers = true }) {
        }

        private class MvcCustomization : ICustomization {
            public void Customize(IFixture fixture) {
                fixture.Customize<ControllerContext>(c => c
                    .Without(x => x.DisplayMode));
            }
        }
    }


    internal static class FixtureExtensions {

        internal static IPostprocessComposer<T> BuildController<T>(this IFixture fixture) where T : Controller {
            return fixture.Build<T>()
                .Without(c => c.ActionInvoker)
                .Without(c => c.ControllerContext)
                .Without(c => c.Resolver)
                .Without(c => c.TempData)
                .Without(c => c.TempDataProvider)
                .Without(c => c.Url)
                .Without(c => c.ValidateRequest)
                .Without(c => c.ValueProvider)
                .Without(c => c.ViewData)
                .Without(c => c.ViewEngineCollection);
        }

        internal static T CreateController<T>(this IFixture fixture) where T : Controller {
            return fixture.BuildController<T>().Create();
        }
    }
}
