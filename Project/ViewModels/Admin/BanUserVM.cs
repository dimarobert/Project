using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Admin {
    public class BanUserVM {

        [Required]
        public int UserProfileId { get; set; }
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Ban expiration")]
        public DateTime BanUntil { get; set; }

    }
}