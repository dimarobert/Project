using System.Collections.Generic;

namespace Project.ViewModels.UserProfile {
    public class GoalVM {
        public int Id { get; set; }

        public string Title { get; set; }

        public int UserProfileId { get; set; }

        public IList<StepVM> Steps { get; set; }

    }
}