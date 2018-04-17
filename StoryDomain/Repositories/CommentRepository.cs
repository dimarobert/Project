using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;

namespace Project.StoryDomain.Repositories {

    public interface ICommentRepository : IEntityRepository<Comment, int> { }

    public class CommentRepository : EntityRepository<Comment, int>, ICommentRepository {

        IStoryContext storyDbContext => context as IStoryContext;

        public CommentRepository(IStoryContext storyDbContext) : base(storyDbContext) { }

    }
}
