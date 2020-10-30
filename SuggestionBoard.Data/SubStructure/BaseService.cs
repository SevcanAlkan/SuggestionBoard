using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SuggestionBoard.Core.ViewModel;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.Helper;
using SuggestionBoard.Core.EntityFramework;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SuggestionBoard.Data.SubStructure
{
    public interface IBaseService<S, L, D>
        where S : SaveVM, ISaveVM, new()
        where L : BaseVM, IBaseVM, new()
        where D : BaseEntity, IBaseEntity, new()
    {
        //IRepository<D> Repository { get; }

        Task<bool> AnyAsync(Guid id);
        Task<L> GetByIdAsync(Guid id);
        Task<IEnumerable<L>> GetAllAsync();
        Task<IList<L>> GetAllAsync(Expression<Func<D, bool>> expr);
        Task<APIResultVM> AddAsync(S model, Guid? userId = null, bool isCommit = true);
        Task<APIResultVM> UpdateAsync(Guid id, S model, Guid? userId = null, bool isCommit = true);
        Task<APIResultVM> DeleteAsync(Guid id, Guid? userId = null, bool isCommit = true);
        Task<APIResultVM> ReverseDeleteAsync(Guid id, Guid? userId, bool isCommit = true);
        Task<APIResultVM> CommitAsync();
    }

    //TODO: remove add and update VMs, the DetailVM will be used

    public class BaseService<S, L, D> : IBaseService<S, L, D>
        where S : SaveVM, ISaveVM, new()
        where L : BaseVM, IBaseVM, new()
        where D : BaseEntity, IBaseEntity, new()
    {
        protected UnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseService<S, L, D>> _logger;

        public BaseService(UnitOfWork uow, IMapper mapper, ILogger<BaseService<S, L, D>> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        protected IRepository<D> Repository
        {
            get
            {
                return _uow.Repository<D>();
            }
        }

        public virtual async Task<bool> AnyAsync(Guid id)
        {
            try
            {
                if (id.IsNull())
                    return false;

                return await _uow.Repository<D>().AnyAysnc(a => a.Id == id);
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return false;
            }
        }
        public virtual async Task<L> GetByIdAsync(Guid id)
        {
            try
            {
                if (id.IsNull())
                    return null;

                return _mapper.Map<L>(await _uow.Repository<D>().GetByID(id));
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }
        public virtual async Task<IEnumerable<L>> GetAllAsync()
        {
            try
            {
                return await _mapper.ProjectTo<L>(Repository.Query()).ToListAsync();
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }
        public virtual async Task<IList<L>> GetAllAsync(Expression<Func<D, bool>> expr)
        {
            try
            {
                return await _mapper.ProjectTo<L>(Repository.Query().Where(expr)).ToListAsync();
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }

        public virtual async Task<APIResultVM> AddAsync(S model, Guid? userId = null, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = _mapper.Map<S, D>(model);
                if (entity.Id == null || entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();

                if (entity is ITableEntity)
                {
                    (entity as ITableEntity).CreateBy = _userId;
                    (entity as ITableEntity).CreateDT = DateTime.Now;
                }

                Repository.Add(entity);

                if (isCommit)
                    await CommitAsync();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
        public virtual async Task<APIResultVM> UpdateAsync(Guid id, S model, Guid? userId = null, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = await _uow.Repository<D>().GetByID(id);
                if (entity.IsNull())
                    return APIResult.CreateVM(false, id);

                entity = _mapper.Map<S, D>(model, entity);

                if (entity is ITableEntity)
                {
                    (entity as ITableEntity).UpdateBy = _userId;
                    (entity as ITableEntity).UpdateDT = DateTime.Now;
                }

                Repository.Update(entity);

                if (isCommit)
                    await CommitAsync();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
        public virtual async Task<APIResultVM> DeleteAsync(Guid id, Guid? userId = null, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = await _uow.Repository<D>().GetByID(id);
                if (entity.IsNull())
                    return APIResult.CreateVM(false, id);

                if (entity is ITableEntity)
                {
                    (entity as ITableEntity).UpdateBy = _userId;
                    (entity as ITableEntity).UpdateDT = DateTime.Now;
                }

                entity.IsDeleted = true;
                Repository.Update(entity);

                if (isCommit)
                    await CommitAsync();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
        public virtual async Task<APIResultVM> ReverseDeleteAsync(Guid id, Guid? userId, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = await _uow.Repository<D>().GetByID(id);
                if (entity.IsNull())
                    return APIResult.CreateVM(false, id);

                if (entity is ITableEntity)
                {
                    (entity as ITableEntity).UpdateBy = _userId;
                    (entity as ITableEntity).UpdateDT = DateTime.Now;
                }

                entity.IsDeleted = false;
                Repository.Update(entity);

                if (isCommit)
                    await CommitAsync();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }

        public virtual async Task<APIResultVM> CommitAsync()
        {
            try
            {
                await _uow.SaveChanges();

                return APIResult.CreateVM(true);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
    }
}
