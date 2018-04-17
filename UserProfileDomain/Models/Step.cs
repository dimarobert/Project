using Project.Core.Enums;
using Project.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.Models
{
    public class Step : ObjectWithState
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        [ForeignKey("Goal")]
        public int GoalId { get; set; }

        public bool IsDone { get; set; }

        public Difficulty Difficulty { get; set; }

        public virtual Goal Goal { get; set; }

    }
}
