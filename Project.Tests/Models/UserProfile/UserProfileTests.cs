using AutoFixture;
using AutoMapper;
using Project.StoryDomain.Models;
using Project.Tests.Utils;
using Project.ViewModels;
using Project.ViewModels.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Models.UserProfile {
    public class UserProfileTests {

        public UserProfileTests() {
            AutoMapperUtil.ConfigureOnce();
        }

    }
}
