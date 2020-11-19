using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Core.ViewModel;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionBoard.Data.Service
{
    public class CategoryService : BaseService<CategorySaveVM, CategoryVM, Category>,
        ICategoryService
    {
        private SuggestionBoardDbContext _con;

        #region Ctor

        public CategoryService(UnitOfWork uow, IMapper mapper,
            ILogger<CategoryService> logger,
            ILogger<IRepository<Category>> repositoryLogger,
            SuggestionBoardDbContext con)
            : base(uow, mapper, logger, repositoryLogger)
        {
            _con = con;
        }

        #endregion

        #region Methods

        public CategoryPaggingListVM GetList(bool showIsDeleted = false,
            int pageNumber = 1, int pageItemCount = 10)
        {
            CategoryPaggingListVM result = new CategoryPaggingListVM();

            result.Records = GetAllAsync(null, true, pageNumber, pageItemCount).Result;

            #region Next Page Check
            var query = Repository.Query(showIsDeleted).AsNoTracking().AsQueryable();

            result.Pagging = new PaggingVM();
            result.Pagging.IsNextPageExist = query.Skip((pageNumber * pageItemCount)).Take(1).Count() == 1;
            #endregion

            var users = _con.Set<User>().AsNoTracking().Select(s => new { s.Id, s.UserName }).ToList();

            foreach (var item in result.Records)
            {
                item.CreateByName = users.Any(a => a.Id == item.CreateById) ? users.Where(a => a.Id == item.CreateById).Select(s => s.UserName).FirstOrDefault() : "";
            }

            return result;
        }

        public async Task<List<CategoryVM>> GetAllAsync(Expression<Func<Category, bool>> expr = null, bool asNoTracking = true, int pageNumber = 1, int pageItemCount = 10)
        {
            try
            {
                var query = Repository.Query().OrderBy(o => o.Name).AsQueryable();

                if (expr != null)
                    query = query.Where(expr);

                if (asNoTracking)
                    query = query.AsNoTracking();

                if (pageNumber > 1)
                    query = query.Skip((pageNumber - 1) * pageItemCount);

                return await _mapper.ProjectTo<CategoryVM>(query.Take(pageItemCount)).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("BaseService.GetAllAsync", e);
                return null;
            }
        }

        public async Task<CategoryDetailVM> GetForEdit(Guid? id)
        {
            CategoryDetailVM vm = new CategoryDetailVM();
            vm.Rec = new CategorySaveVM();

            if (id.IsNullOrEmpty())
                return vm;

            var record = await Repository.GetByIDAysnc(id.Value);

            if (record != null)
            {
                var user = _con.Set<User>().AsNoTracking().Where(a => a.Id == record.CreateBy).Select(s => new { s.Id, s.UserName }).FirstOrDefault();

                vm.Id = id.Value;
                vm.Rec = _mapper.Map<CategorySaveVM>(record);
                vm.Rec.CreateByName = user.UserName;
            }

            return vm;
        }

        public List<SelectListVM> GetSelectList()
        {
            return _con.Set<Category>().Where(a => !a.IsDeleted)
                .Select(s => new SelectListVM()
                {
                    Id = s.Id,
                    Text = s.Name
                }).OrderBy(o => o.Text).AsNoTracking().ToList();
        }

        #endregion
    }

    public interface ICategoryService : IBaseService<CategorySaveVM, CategoryVM, Category>
    {
        Task<List<CategoryVM>> GetAllAsync(Expression<Func<Category, bool>> expr = null, bool asNoTracking = true, int pageNumber = 1, int pageItemCount = 10);
        CategoryPaggingListVM GetList(bool showIsDeleted = false, int pageNumber = 1, int pageItemCount = 10);
        Task<CategoryDetailVM> GetForEdit(Guid? id);
        List<SelectListVM> GetSelectList();
    }
}
