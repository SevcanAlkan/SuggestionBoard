using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Validation;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionBoard.Data.Service
{
    public class SuggestionService : BaseService<SuggestionSaveVM, SuggestionVM, Suggestion>,
    ISuggestionService
    {
        private SuggestionBoardDbContext _con;

        #region Ctor

        public SuggestionService(UnitOfWork uow, IMapper mapper, 
            ILogger<SuggestionService> logger,
            ILogger<IRepository<Suggestion>> repositoryLogger,
            SuggestionBoardDbContext context)
            : base(uow, mapper, logger, repositoryLogger)
        {
            _con = context;
        }

        #endregion

        #region Methods

        public SuggestionPaggingListVM GetList(bool showIsDeleted = false, string searchText = "", string sortOrder = "", int pageNumber = 1, int pageItemCount = 10)
        {            
            var query = Repository.Query(showIsDeleted).AsNoTracking();

            if (!searchText.IsNullOrEmpty())
            {
                searchText = searchText.Trim().ToUpper();
                query = query.Where(a => a.Title.ToUpper().Contains(searchText)
                    || a.Description.ToUpper().Contains(searchText)).AsQueryable();
            }

            var selectedQuery = query.Include(s => s.SuggestionComments).Select(s => new SuggestionVM()
                {
                    Id = s.Id,
                    CreateDateTime = s.CreateDT,
                    Description = s.Description,
                    Status = s.Status,
                    LikeAmount = s.LikeAmount,
                    DislikeAmount = s.DislikeAmount,
                    Title = s.Title,
                    TotalReaction = s.DislikeAmount + s.LikeAmount + ((s.SuggestionComments != null ? s.SuggestionComments.Count : 0) * 2),
                    CommentCount = s.SuggestionComments != null ? s.SuggestionComments.Count : 0,
                    CreateById = s.CreateBy,
                    CreateByName = ""

                });

            SuggestionPaggingListVM result = new SuggestionPaggingListVM();

            switch (sortOrder)
            {
                case "newest":
                    selectedQuery = selectedQuery.OrderBy(o => o.CreateDateTime);
                    break;
                case "comment":
                    selectedQuery = selectedQuery.OrderByDescending(o => o.CommentCount);
                    break;
                case "like":
                    selectedQuery = selectedQuery.OrderByDescending(o => o.LikeAmount);
                    break;
                case "reaction":
                    selectedQuery = selectedQuery.OrderByDescending(o => o.TotalReaction);
                    break;
                default:
                    selectedQuery = selectedQuery.OrderByDescending(o => o.CreateDateTime);
                    break;
            }

            if (pageNumber > 1)
                selectedQuery = selectedQuery.Skip((pageNumber - 1) * pageItemCount);

            result.Records = selectedQuery.Take(pageItemCount).ToList();

            #region Next Page Check
            query = Repository.Query(showIsDeleted).AsNoTracking().AsQueryable();

            if (!searchText.IsNullOrEmpty())
            {
                query = query.Where(a => a.Title.ToUpper().Contains(searchText)
                    || a.Description.ToUpper().Contains(searchText)).AsQueryable();
            }

            result.IsNextPageExist = query.Skip((pageNumber * pageItemCount)).Take(1).Count() == 1;
            #endregion

            var users = _con.Set<User>().AsNoTracking().Select(s => new { s.Id, s.UserName }).ToList();

            foreach (var item in result.Records)
            {
                item.CreateByName = users.Any(a => a.Id == item.CreateById) ? users.Where(a => a.Id == item.CreateById).Select(s => s.UserName).FirstOrDefault() : "";
            }

            return result;
        }

        #endregion
    }

    public interface ISuggestionService : IBaseService<SuggestionSaveVM, SuggestionVM, Suggestion>
    {
        SuggestionPaggingListVM GetList(bool showIsDeleted = false, string searchText = "", string sortOrder = "", int pageNumber = 1, int pageItemCount = 10);
    }
}
