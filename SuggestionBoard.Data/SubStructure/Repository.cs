using Microsoft.EntityFrameworkCore;
using SuggestionBoard.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionBoard.Data.SubStructure
{
    public interface IRepository<T>
    {
        Task<T> GetByID(Guid id);
        IQueryable<T> Get();
        IQueryable<T> Query(bool isDeleted = false);
        void Add(T entity);
        void Update(T entity);
        Task<bool> AnyAysnc(Expression<Func<T, bool>> expr);
    }

    public class Repository<T> : IRepository<T>
          where T : BaseEntity
    {
        private SuggestionBoardDbContext con;
        public Repository(SuggestionBoardDbContext context)
        {
            con = context;
        }
        public SuggestionBoardDbContext Context
        {
            get { return con; }
            set { con = value; }

        }
        public virtual async Task<T> GetByID(Guid id)
        {
            return await con.Set<T>().FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }
        public IQueryable<T> Get()
        {
            return con.Set<T>().AsQueryable();
        }
        public virtual void Add(T entity)
        {
            con.Set<T>().Add(entity);
        }
        public virtual void Update(T entity)
        {
            con.Entry(entity).State = EntityState.Modified;
        }
        public IQueryable<T> Query(bool isDeleted = false)
        {
            return con.Set<T>().AsNoTracking().Where(x => !x.IsDeleted || x.IsDeleted == isDeleted).AsQueryable();
        }
        public Task<bool> AnyAysnc(Expression<Func<T, bool>> expr)
        {
            return con.Set<T>().AsNoTracking().AnyAsync(expr);
        }
    }
}
