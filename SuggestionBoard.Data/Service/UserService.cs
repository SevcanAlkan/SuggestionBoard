using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Core.Helper;
using SuggestionBoard.Core.ViewModel;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Data.ViewModel;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionBoard.Data.Service
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly SuggestionBoardDbContext _con;
        private readonly ISuggestionService _suggestionService;
        private readonly ISuggestionCommentService _suggestionCommentService;
        private readonly ISuggestionReactionService _suggestionReactionService;

        #region Ctor

        public UserService(IMapper mapper,
            ILogger<UserService> logger,
            SuggestionBoardDbContext context,
            ISuggestionService suggestionService,
            ISuggestionCommentService suggestionCommentService,
            ISuggestionReactionService suggestionReactionService)
        {
            _con = context;
            _mapper = mapper;
            _logger = logger;
            _suggestionService = suggestionService;
            _suggestionCommentService = suggestionCommentService;
            _suggestionReactionService = suggestionReactionService;
        }

        #endregion

        #region Methods

        public async Task<APIResultVM> GetProfileData(Guid userId, Guid currentUserId, string sortOrder = "", int pageNumber = 1, int pageItemCount = 10, Guid? categoryId = null)
        {
            if (userId == null || userId == Guid.Empty)
                APIResult.CreateVM();

            var user = _con.Set<User>().AsNoTracking().Where(a => a.Id == userId).FirstOrDefault();
            if(user == null)
                APIResult.CreateVM();

            ProfileVM model = new ProfileVM();
            _mapper.Map<ProfileVM>(user);

            model.Suggestion = new SuggestionPaggingListVM();
            model.Suggestion.Pagging = new PaggingVM();

            model.Suggestion.Records = _suggestionService.GetAllAsync(a => a.CreateBy == userId, true, sortOrder, pageNumber, pageItemCount, categoryId).Result;
            model.Comments = await _suggestionCommentService.GetCommentsOfUser(userId);
            model.Reactions = await _suggestionReactionService.GetReactionsOfUser(userId);

            model.UserId = userId;
            model.PictureUrl = user.PictureUrl;
            model.CurrentUserId = currentUserId;
            model.EMail = user.Email;
            model.SuggetionCount = _con.Set<Suggestion>().Where(a => !a.IsDeleted && a.CreateBy == userId).Count();

            model.Suggestion.Pagging.ActionName = "Profile";
            model.Suggestion.Pagging.ControllerName = "User";
            var query = _con.Set<Suggestion>().Where(a=> !a.IsDeleted && a.CreateBy == userId).AsNoTracking().AsQueryable();
            model.Suggestion.Pagging.IsNextPageExist = query.Skip((pageNumber * pageItemCount)).Take(1).Count() == 1;

            foreach (var item in model.Suggestion.Records)
            {
                item.CreateByName = user.Email;
            }

            return APIResult.CreateVMWithRec<ProfileVM>(model, true, userId);
        }

        #endregion
    }

    public interface IUserService
    {
        Task<APIResultVM> GetProfileData(Guid userId, Guid currentUserId, string sortOrder = "", int pageNumber = 1, int pageItemCount = 10, Guid? categoryId = null);
    }
}
