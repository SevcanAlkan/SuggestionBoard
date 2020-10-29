using AutoMapper;
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

        public SuggestionReactionService(UnitOfWork _uow, IMapper _mapper)
            : base(_uow, _mapper)
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
