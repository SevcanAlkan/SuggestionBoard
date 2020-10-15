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
            CreateMap<Suggestion, SuggestionAddVM>();
            CreateMap<SuggestionAddVM, Suggestion>();
            CreateMap<Suggestion, SuggestionUpdateVM>();
            CreateMap<SuggestionUpdateVM, Suggestion>();

            CreateMap<SuggestionComment, SuggestionCommentVM>().ReverseMap();
            CreateMap<SuggestionComment, SuggestionCommentAddVM>();
            CreateMap<SuggestionCommentAddVM, SuggestionComment>();
            CreateMap<SuggestionComment, SuggestionCommentUpdateVM>();
            CreateMap<SuggestionCommentUpdateVM, SuggestionComment>();

            CreateMap<SuggestionReaction, SuggestionReactionVM>().ReverseMap();
            CreateMap<SuggestionReaction, SuggestionReactionAddVM>();
            CreateMap<SuggestionReactionAddVM, SuggestionReaction>();
            CreateMap<SuggestionReaction, SuggestionReactionUpdateVM>();
            CreateMap<SuggestionReactionUpdateVM, SuggestionReaction>();
        }
    }
}
