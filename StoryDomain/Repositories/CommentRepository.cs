using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;

namespace Project.StoryDomain.Repositories {

    public interface ICommentRepository : IEntityRepository<Comment> { }

    public class CommentRepository : ICommentRepository {

        IStoryContext storyDbContext { get; }

        public IList<Comment> All => throw new NotImplementedException();

        public Task<IList<Comment>> AllAsync => throw new NotImplementedException();

        public CommentRepository(IStoryContext storyDbContext) {
            this.storyDbContext = storyDbContext ?? throw new ArgumentNullException(nameof(storyDbContext));
        }

        public IList<Comment> AllIncluding(params Expression<Func<Comment, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public Task<IList<Comment>> AllIncludingAsync(params Expression<Func<Comment, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public IList<Comment> Get(params Expression<Func<Comment, bool>>[] filters) {
            throw new NotImplementedException();
        }

        public Task<IList<Comment>> GetAsync(params Expression<Func<Comment, bool>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<Comment> GetIncluding(Expression<Func<Comment, bool>>[] filters, Expression<Func<Comment, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public Task<IList<Comment>> GetIncludingAsync(Expression<Func<Comment, bool>>[] filters, Expression<Func<Comment, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateGraph(Comment entity) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Comment entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }

        public Task SaveAsync() {
            throw new NotImplementedException();
        }

        public void Delete(Comment entity) {
            throw new NotImplementedException();
        }
    }
}
