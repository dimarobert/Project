using Project.Core.DbContext;
using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {

    public interface IStoryUnitOfWork : IUnitOfWork {
        IStoryRepository Stories { get; }
        ICommentRepository Comments { get; }
        IHashtagRepository Hashtags { get; }
        IGroupRepository Groups { get; }
    }

    public class StoryUnitOfWork : UnitOfWork, IStoryUnitOfWork {

        IStoryContext storyContext => context as IStoryContext;

        public IStoryRepository Stories { get; }
        public ICommentRepository Comments { get; }

        public IHashtagRepository Hashtags { get; }

        public IGroupRepository Groups { get; }

        public StoryUnitOfWork(IStoryContext context,
            IStoryRepository stories,
            ICommentRepository comments,
            IHashtagRepository hashtags,
            IGroupRepository groups
            ) : base(context) {

            Stories = stories;
            Comments = comments;
            Hashtags = hashtags;
            Groups = groups;
        }
    }

}
