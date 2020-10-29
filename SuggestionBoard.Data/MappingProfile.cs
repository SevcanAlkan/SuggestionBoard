using AutoMapper;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Suggestion, SuggestionVM>().ReverseMap();
            CreateMap<Suggestion, SuggestionSaveVM>().ReverseMap();

            CreateMap<SuggestionComment, SuggestionCommentVM>().ReverseMap();
            CreateMap<SuggestionComment, SuggestionCommentSaveVM>().ReverseMap();

            CreateMap<SuggestionReaction, SuggestionReactionVM>().ReverseMap();
            CreateMap<SuggestionReaction, SuggestionReactionSaveVM>().ReverseMap();
        }
    }
}
