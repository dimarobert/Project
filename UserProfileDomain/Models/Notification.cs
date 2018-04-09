using Project.Account.Models;
using Project.Core.Models;
using System;

namespace Project.UserProfileDomain.Models {
    public class Notification : ObjectWithState {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }


        public UserInfo User { get; set; }
    }
}
