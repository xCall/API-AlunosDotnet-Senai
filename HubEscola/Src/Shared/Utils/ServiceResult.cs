using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;

namespace HubEscola.Src.Shared.Utils
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public Exception Error { get; set; }
        public bool IsNotFound { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public static ServiceResult SuccessResult()
        {
            ServiceResult result = new() {
                Success = true,
            };

            return result;
        
        }

        public static ServiceResult SuccessResultData(object returnObject)
        {
            ServiceResult result = new ServiceResult
            {
                Success = true,
                Data = returnObject
            };

            return result;

        }

        public static ServiceResult FailureResult(Exception ex)
        {
            return new ServiceResult { Success = false, Error = ex };
        }


        public static ServiceResult NotFound(string message)
        { 
            return new ServiceResult { IsNotFound = true, Message = message };
        }

       
    }
}
