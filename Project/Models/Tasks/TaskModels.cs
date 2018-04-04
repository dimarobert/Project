using Project.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.Tasks {
    public class Task {

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }


        public virtual UserInfo User { get; set; }


    }
}