using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Project.StoryDomain.Models;

namespace Project.StoryDomain.Services {


    public interface IStoryService {
        IList<string> ExtractHashtags(Story story);
    }

    public class StoryService : IStoryService {

        private Regex hashtagRegex;

        public StoryService() {
            hashtagRegex = new Regex(@"(#[^\s#]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public IList<string> ExtractHashtags(Story story) {
            var matches = hashtagRegex.Matches(story.Content);

            return matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
        }
    }
}
