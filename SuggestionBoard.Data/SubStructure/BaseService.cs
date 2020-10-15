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

namespace SuggestionBoard.Data.SubStructure
{
    public interface ICRUDService<A, U, G>
       where A : AddVM, IAddVM, new()
       where U : UpdateVM, IUpdateVM, new()
       where G : BaseVM, IBaseVM, new()
    {
        Task<G> GetByIdAsync(Guid id);
        IList<G> GetAll();

        Task<APIResultVM> Add(A model, Guid? userId = null, bool isCommit = true);
        Task<APIResultVM> Update(Guid id, U model, Guid? userId = null, bool isCommit = true);
        Task<APIResultVM> Delete(Guid id, Guid? userId = null, bool isCommit = true);
        Task<APIResultVM> ReverseDelete(Guid id, Guid? userId, bool isCommit = true);
        G GetById(Guid id);
        Task<APIResultVM> Commit();
    }

    public interface IBaseService<A, U, G, D> : ICRUDService<A, U, G>
        where A : AddVM, IAddVM, new()
        where D : BaseEntity, IBaseEntity, new()
        where U : UpdateVM, IUpdateVM, new()
        where G : BaseVM, IBaseVM, new()
    {
        IRepository<D> Repository { get; }
        IList<G> GetAll(Expression<Func<D, bool>> expr);
    }

    public class BaseService<A, U, G, D> : IBaseService<A, U, G, D>
        where A : AddVM, IAddVM, new()
        where D : BaseEntity, IBaseEntity, new()
        where U : UpdateVM, IUpdateVM, new()
        where G : BaseVM, IBaseVM, new()
    {
        protected UnitOfWork uow;
        protected readonly IMapper mapper;

        public BaseService(UnitOfWork _uow, IMapper _mapper)
        {
            uow = _uow;
            mapper = _mapper;
        }

        public IRepository<D> Repository
        {
            get
            {
                return uow.Repository<D>();
            }
        }

        public virtual async Task<G> GetByIdAsync(Guid id)
        {
            try
            {
                if (id.IsNull())
                    return null;

                return mapper.Map<G>(await uow.Repository<D>().GetByID(id));
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }
        public virtual G GetById(Guid id)
        {
            try
            {
                D dm = Repository.Query().Where(x => x.Id == id).FirstOrDefault();
                if (dm.IsNull())
                    return null;

                G vm = mapper.Map<D, G>(dm);

                return vm;
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }
        public virtual IList<G> GetAll()
        {
            try
            {
                return mapper.ProjectTo<G>(Repository.Query()).ToList();
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }
        public virtual IList<G> GetAll(Expression<Func<D, bool>> expr)
        {
            try
            {
                return mapper.ProjectTo<G>(Repository.Query().Where(expr)).ToList();
            }
            catch (Exception e)
            {
                APIResult.CreateVMWithError(e);
                return null;
            }
        }

        public virtual async Task<APIResultVM> Add(A model, Guid? userId = null, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = mapper.Map<A, D>(model);
                if (entity.Id == null || entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();

                if (entity is ITableEntity)
                {
                    (entity as ITableEntity).CreateBy = _userId;
                    (entity as ITableEntity).CreateDT = DateTime.Now;
                }

                Repository.Add(entity);

                if (isCommit)
                    await Commit();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
        public virtual async Task<APIResultVM> Update(Guid id, U model, Guid? userId = null, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = await uow.Repository<D>().GetByID(id);
                if (entity.IsNull())
                    return APIResult.CreateVM(false, id);

                entity = mapper.Map<U, D>(model, entity);

                if (entity is ITableEntity)
                {
                    (entity as ITableEntity).UpdateBy = _userId;
                    (entity as ITableEntity).UpdateDT = DateTime.Now;
                }

                Repository.Update(entity);

                if (isCommit)
                    await Commit();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
        public virtual async Task<APIResultVM> Delete(Guid id, Guid? userId = null, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = await uow.Repository<D>().GetByID(id);
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
                    await Commit();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
        public virtual async Task<APIResultVM> ReverseDelete(Guid id, Guid? userId, bool isCommit = true)
        {
            try
            {
                Guid _userId = userId == null ? Guid.Empty : userId.Value;

                D entity = await uow.Repository<D>().GetByID(id);
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
                    await Commit();

                return APIResult.CreateVMWithRec(entity, true, entity.Id);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }

        public virtual async Task<APIResultVM> Commit()
        {
            try
            {
                await uow.SaveChanges();

                return APIResult.CreateVM(true);
            }
            catch (Exception e)
            {
                return APIResult.CreateVMWithError(e);
            }
        }
    }
}
