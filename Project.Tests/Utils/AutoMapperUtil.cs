using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests.Utils {
    public static class AutoMapperUtil {

        static Lazy<bool> isConfigured = new Lazy<bool>(() => {

            AutoMapperConfig.Configure();

            return true;
        }, true);

        public static void ConfigureOnce() {
            var x = isConfigured.Value;
        }

    }
}
