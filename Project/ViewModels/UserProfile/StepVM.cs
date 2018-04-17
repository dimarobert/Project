using Project.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.UserProfile
{
    public class StepVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int GoalId { get; set; }

        public bool IsDone { get; set; }

        public Difficulty Difficulty { get; set; }

    }
}