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
            CreateMap<Suggestion, SuggestionVM>()
                .ForMember(m => m.CreateById, o => o.MapFrom(s => s.CreateBy));

            CreateMap<SuggestionVM, Suggestion>()
                .ForMember(m => m.CreateBy, o => o.Ignore())
                .ForMember(m => m.LikeAmount, o => o.Ignore())
                .ForMember(m => m.DislikeAmount, o => o.Ignore());

            CreateMap<SuggestionVM, SuggestionSaveVM>()
                .ForMember(m => m.CreateBy, o => o.MapFrom(s => s.CreateById));

            CreateMap<SuggestionSaveVM, Suggestion>()
                .ForMember(m => m.CreateBy, o => o.Ignore())
                .ForMember(m => m.CreateDT, o => o.Ignore());
            CreateMap<Suggestion, SuggestionSaveVM>();


            CreateMap<SuggestionComment, SuggestionCommentVM>();
               // .ForMember(m => m.CreateBy, o => o.MapFrom(s => s.CreateBy));
            CreateMap<SuggestionCommentVM, SuggestionCommentSaveVM>().ReverseMap();
            CreateMap<SuggestionComment, SuggestionCommentSaveVM>().ReverseMap();

            CreateMap<SuggestionReaction, SuggestionReactionVM>().ReverseMap();
            CreateMap<SuggestionReactionVM, SuggestionReactionSaveVM>().ReverseMap();
            CreateMap<SuggestionReaction, SuggestionReactionSaveVM>().ReverseMap();
        }
    }
}
