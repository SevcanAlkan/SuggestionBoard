using AutoMapper;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data.Service
{
    public class SuggestionService : BaseService<SuggestionAddVM,
    SuggestionUpdateVM,
    SuggestionVM,
    Suggestion>,
    ISuggestionService
    {
        #region Ctor

        public SuggestionService(UnitOfWork _uow, IMapper _mapper)
            : base(_uow, _mapper)
        {

        }

        #endregion

        #region Methods


        #endregion
    }

    public interface ISuggestionService : IBaseService<SuggestionAddVM, SuggestionUpdateVM, SuggestionVM, Suggestion>
    {
    }
}
