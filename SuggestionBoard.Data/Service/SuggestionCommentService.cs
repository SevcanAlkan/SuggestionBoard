using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class SuggestionCommentService : BaseService<SuggestionCommentSaveVM, SuggestionCommentVM, SuggestionComment>,
        ISuggestionCommentService
    {
        #region Ctor

        public SuggestionCommentService(UnitOfWork uow, IMapper mapper,
            ILogger<SuggestionCommentService> logger,
            ILogger<IRepository<SuggestionComment>> repositoryLogger)
            : base(uow, mapper, logger, repositoryLogger)
        {

        }

        #endregion

        #region Methods

        public Task<List<ProfileCommentVM>> GetCommentsOfUser(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                return Task.Run(() => new List<ProfileCommentVM>());

            return Repository.Query().Where(a => a.CreateBy == userId).Include(t => t.Suggestion)
                .Select(s => new ProfileCommentVM()
                {
                    CommentText = s.Text,
                    CreateDT = s.CreateDT,
                    SuggestionId = s.SuggestionId,
                    SuggestionTitle = s.Suggestion != null ? s.Suggestion.Title : "-"
                }).OrderByDescending(o => o.CreateDT).ToListAsync();
        }

        #endregion
    }

    public interface ISuggestionCommentService : IBaseService<SuggestionCommentSaveVM, SuggestionCommentVM, SuggestionComment>
    {
        Task<List<ProfileCommentVM>> GetCommentsOfUser(Guid userId);
    }
}
