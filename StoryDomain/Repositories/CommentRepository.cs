using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;

namespace Project.StoryDomain.Repositories {

    public interface ICommentRepository : IEntityRepository<Comment> { }

    public class CommentRepository : ICommentRepository {

        IStoryContext storyDbContext { get; }

        public IList<Comment> All => throw new NotImplementedException();

        public CommentRepository(IStoryContext storyDbContext) {
            this.storyDbContext = storyDbContext ?? throw new ArgumentNullException(nameof(storyDbContext));
        }

        public IList<Comment> AllIncluding(params Expression<Func<Comment, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void Delete(Comment entity) {
            throw new NotImplementedException();
        }

        public IList<Comment> Get(params Expression<Func<Comment, object>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<Comment> GetIncluding(Expression<Func<Comment, object>>[] filters, Expression<Func<Comment, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Comment entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }
    }
}
