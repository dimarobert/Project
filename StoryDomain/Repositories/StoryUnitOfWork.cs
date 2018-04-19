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
    }

    public class StoryUnitOfWork : UnitOfWork, IStoryUnitOfWork {

        IStoryContext storyContext => context as IStoryContext;

        public IStoryRepository Stories { get; }
        public ICommentRepository Comments { get; }

        public StoryUnitOfWork(IStoryContext context, IStoryRepository stories, ICommentRepository comments) : base(context) {
            Stories = stories;
            Comments = comments;
        }
    }

}
