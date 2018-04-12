using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class UserProfileVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string AboutMe { get; set; }

        public DateTime BirthDate { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

    }
}