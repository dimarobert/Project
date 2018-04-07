using Project.StoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Project.Tests.Models.Tasks {
    public class StoryTests {

        [Fact]
        public void ShouldHaveStoryModel() {
            var task = new Story();
        }

    }
}
