using System.Threading.Tasks;

namespace MovieSearchApp.Services.Rest
{
   public interface IRestService
   {
        Task<(ResultStatus status, TResponse payload, string rawResponse)> GetAsync<TResponse>(string path);
        Task<(ResultStatus status, TResponse payload, string rawResponse)> PostAsync<TRequest, TResponse>(TRequest request, string path);
   }

   public enum ResultStatus
   {
        Success = 0,
        ConnectionFailed,
        Unauthorized,
        BadResponse,
        BadPayload,
        // Map HttpStatus codes to additional enum members here ...
        Other
    }

 }