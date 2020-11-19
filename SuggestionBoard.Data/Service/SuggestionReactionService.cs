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
    public class SuggestionReactionService : BaseService<SuggestionReactionSaveVM, SuggestionReactionVM, SuggestionReaction>,
        ISuggestionReactionService
    {
        #region Ctor

        public SuggestionReactionService(UnitOfWork uow, IMapper mapper,
            ILogger<SuggestionReactionService> logger,
            ILogger<IRepository<SuggestionReaction>> repositoryLogger)
            : base(uow, mapper, logger, repositoryLogger)
        {

        }

        #endregion

        #region Methods

        public Task<List<ProfileReactionVM>> GetReactionsOfUser(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                return Task.Run(() => new List<ProfileReactionVM>());

            return Repository.Query().Where(a => a.CreateBy == userId).Include(t => t.Suggestion)
                .Select(s => new ProfileReactionVM()
                {
                    Reaction = s.Reaction,
                    CreateDT = s.CreateDT,
                    SuggestionId = s.SuggestionId,
                    SuggestionTitle = s.Suggestion != null ? s.Suggestion.Title : "-"
                }).OrderByDescending(o => o.CreateDT).ToListAsync();
        }

        #endregion
    }

    public interface ISuggestionReactionService : IBaseService<SuggestionReactionSaveVM, SuggestionReactionVM, SuggestionReaction>
    {
        Task<List<ProfileReactionVM>> GetReactionsOfUser(Guid userId);
    }
}
