using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SuggestionBoard.Core.ViewModel;

namespace SuggestionBoard.Core.Helper
{
    public static class APIResult
    {
        public static APIResultVM CreateVM(bool isSuccessful = false, Guid? recId = null)
        {
            var vm = new APIResultVM()
            {
                Result = isSuccessful,
                RecId = recId
            };
            return vm;
        }

        public static APIResultVM CreateVMWithRec<T>(T rec, bool isSuccessful = false, Guid? recId = null)
        {
            var vm = new APIResultVM()
            {
                Result = isSuccessful,
                RecId = recId,
                Rec = rec
            };

            return vm;
        }

        public static APIResultVM CreateVMWithError(Exception e, APIResultVM vm = null)
        {
            if (vm == null)
                vm = new APIResultVM()
                {
                    Result = false,
                    RecId = null
                };

            if (vm.Errors == null)
                vm.Errors = new List<APIErrorVM>();

            vm.Errors.Add(new APIErrorVM()
            {
                DateTime = DateTime.Now,
                ErrorId = Guid.NewGuid(),
                Message = e.Message,
                Source = e.Source,
                StackTrace = e.StackTrace,
                InnerException = e.InnerException != null ? (
                    "Message: " + (e.InnerException.Message != null ? e.InnerException.Message : "") +
                    "Source: " + (e.InnerException.Source != null ? e.InnerException.Source : "") +
                    "Stack Trace: " + (e.InnerException.StackTrace != null ? e.InnerException.StackTrace : "")
                ) : ""
            });

            return vm;
        }

    }
}
