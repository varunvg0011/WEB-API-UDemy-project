using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Villa_Utility;
using Villa_WebApp.Models;
using Villa_WebApp.Services.IServices;

namespace Villa_WebApp.Services
{
    //Here we are implementing basic generic service 
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get ; set ; }
        //In order to call the API, we are using HTTPClientFactory which is already registered
        //and we are using this in dependency injection and using this service.
        //More about IHTTPClientFactory : https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        //we use this HttpClient to actually call the API. This client is of type IHttpClientFactory
        public IHttpClientFactory httpClientFactory { get; set ; }
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.responseModel = new();
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                //creating a client with random name
                //this client is created using using variable of type IHttpClientFactory
                //and given a name as MagicAPI

                var client = httpClientFactory.CreateClient("MagicAPI");

                //next we need a new HttpRequest message and create a new HttpRequest message
                //using its object. On that message we have to configure things such as in header types
                //(we use header types while making requests as objects and we are configuring them here)
                //First is Accept and then content-Type
                
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept" , "application/json");
                //After that we need the URL where we make the call to the API
                message.RequestUri = new Uri(apiRequest.Url);

                //when we are creating or updating or in general calling such API through which we
                //are making an action then we are going to have some data and we need to serialize it
                if(apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;                       
                }

                //now once we have the request configured completely like above, we need to send it,
                //we recieve a response and to catch that response, we first create a new object of 
                //HttpResponseMessage and decalre as null as default and give it value after that

                HttpResponseMessage apiResponse = null;

                //Then finally, we are calling the API End point by making the call using SendAsync
                //method and pass the message as parameter that we configured in above code
                apiResponse = await client.SendAsync(message);


                //after we send the apiResponse, we need to extract the APicontent from there
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                //then we need to deserialize that content
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }

            //incase of exception encountered, we will create the variable DTO of type APIResponse
            //and we will catch that error in ErrorMessage
            catch (Exception ex)
            {

                var dto = new APIResponse
                {
                    ErrorMessages = new List<String> { ex.Message.ToString() },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
