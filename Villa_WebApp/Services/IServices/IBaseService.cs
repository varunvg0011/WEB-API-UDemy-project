using Villa_WebApp.Models;

namespace Villa_WebApp.Services.IServices
{
    //With the help of this base service, we are going to 
    public interface IBaseService
    {
        //This is for fetching the response
        APIResponse responseModel { get; set; }

        //this is for creating the Send API request/calls to our API. Our API expects an Api request of
        //APIRequestType type and that way we can be generic and pass the API request model

        Task<T> SendAsync<T>(ApiRequest request);
    }
}
