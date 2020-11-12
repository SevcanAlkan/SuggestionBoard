using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
          where T : BaseEntity, IBaseEntity, new()
    {
        private readonly ILogger<IRepository<T>> _logger;
        private SuggestionBoardDbContext con;
        public Repository(SuggestionBoardDbContext context, ILogger<IRepository<T>> logger)
        {
            con = context;
            _logger = logger;
        }
        public SuggestionBoardDbContext Context
        {
            get { return con; }
            set { con = value; }

        }
        public virtual async Task<T> GetByID(Guid id)
        {
            try
            {
                return await con.Set<T>().FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Repository.GetByID", ex);
                return null;
            }
        }
        public IQueryable<T> Get()
        {
            try
            {
                return con.Set<T>().AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError("Repository.Get", ex);
                return null;
            }
        }
        public virtual void Add(T entity)
        {
            try
            {
                con.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("Repository.Add", ex);
            }
        }
        public virtual void Update(T entity)
        {
            try
            {
                con.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError("Repository.Update", ex);
            }
        }
        public IQueryable<T> Query(bool isDeleted = false)
        {
            try
            {
                return con.Set<T>().Where(x => !x.IsDeleted || x.IsDeleted == isDeleted).AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError("Repository.Query", ex);
                return null;
            }
        }
        public Task<bool> AnyAysnc(Expression<Func<T, bool>> expr)
        {
            try
            {
                return con.Set<T>().AsNoTracking().AnyAsync(expr);
            }
            catch (Exception ex)
            {
                _logger.LogError("Repository.AnyAysnc", ex);
                return null;
            }
        }
    }
}
