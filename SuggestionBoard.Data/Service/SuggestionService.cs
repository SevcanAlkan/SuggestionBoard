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
    public class SuggestionService : BaseService<SuggestionSaveVM, SuggestionVM, Suggestion>,
    ISuggestionService
    {
        #region Ctor

        public SuggestionService(UnitOfWork uow, IMapper mapper, 
            ILogger<SuggestionService> logger,
            ILogger<IRepository<Suggestion>> repositoryLogger)
            : base(uow, mapper, logger, repositoryLogger)
        {

        }

        #endregion

        #region Methods


        #endregion
    }

    public interface ISuggestionService : IBaseService<SuggestionSaveVM, SuggestionVM, Suggestion>
    {
    }
}
