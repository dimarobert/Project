using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ViewModels.UserProfile {
    public class InterestVM
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}