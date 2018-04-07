using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Project.StoryDomain.Models {
    public class Story {

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }


        public virtual UserInfoRef User { get; set; }

    }
}
