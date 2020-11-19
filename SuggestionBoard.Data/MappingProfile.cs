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
                .ForMember(m => m.CreateById, o => o.MapFrom(s => s.CreateBy))
                .ForMember(m => m.CreateDateTime, o => o.MapFrom(s => s.CreateDT))
                .ForMember(m => m.TotalReaction, o => o.MapFrom(s => s.DislikeAmount + s.LikeAmount + ((s.SuggestionComments != null ? s.SuggestionComments.Count : 0) * 2)))
                .ForMember(m => m.CommentCount, o => o.MapFrom(s => s.SuggestionComments != null ? s.SuggestionComments.Count : 0));

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

            CreateMap<User, ProfileVM>();
        }
    }
}
