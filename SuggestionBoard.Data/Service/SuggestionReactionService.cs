using AutoMapper;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data.Service
{
    public class SuggestionReactionService : BaseService<SuggestionReactionSaveVM, SuggestionReactionVM, SuggestionReaction>,
        ISuggestionReactionService
    {
        #region Ctor

        public SuggestionReactionService(UnitOfWork uow, IMapper mapper, ILogger<SuggestionReactionService> logger)
            : base(uow, mapper, logger)
        {

        }

        #endregion

        #region Methods


        #endregion
    }

    public interface ISuggestionReactionService : IBaseService<SuggestionReactionSaveVM, SuggestionReactionVM, SuggestionReaction>
    {
    }
}
