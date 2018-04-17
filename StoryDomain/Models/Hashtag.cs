using Project.Core.Models;

namespace Project.StoryDomain.Models {
    public class Hashtag : ObjectWithState, IEntity<int> {
        public int Id { get; set; }

        public string Value { get; set; }

        public int UsageCount { get; set; }
    }
}
