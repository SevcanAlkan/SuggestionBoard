using AutoMapper;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data.Service
{
    public class SuggestionCommentService : BaseService<SuggestionCommentAddVM,
        SuggestionReactionUpdateVM,
        SuggestionCommentVM,
        SuggestionComment>,
        ISuggestionCommentService
    {
        #region Ctor

        public SuggestionCommentService(UnitOfWork _uow, IMapper _mapper)
            : base(_uow, _mapper)
        {

        }

        #endregion

        #region Methods


        #endregion
    }

    public interface ISuggestionCommentService : IBaseService<SuggestionCommentAddVM, SuggestionReactionUpdateVM, SuggestionCommentVM, SuggestionComment>
    {
    }
}
