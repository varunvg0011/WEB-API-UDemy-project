using System.Net;

namespace Villa_WebAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get;set; }
        public object Response { get; set; }
    }
}
