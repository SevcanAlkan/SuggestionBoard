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
    public class SuggestionCommentService : BaseService<SuggestionCommentSaveVM, SuggestionCommentVM, SuggestionComment>,
        ISuggestionCommentService
    {
        #region Ctor

        public SuggestionCommentService(UnitOfWork uow, IMapper mapper, ILogger<SuggestionCommentService> logger)
            : base(uow, mapper, logger)
        {

        }

        #endregion

        #region Methods


        #endregion
    }

    public interface ISuggestionCommentService : IBaseService<SuggestionCommentSaveVM, SuggestionCommentVM, SuggestionComment>
    {
    }
}
