using static Villa_Utility.StaticDetails;

namespace Villa_WebApp.Models
{
    public class ApiRequest
    {
        //ApiType ka enum bana liya. And enum banaya isliye taaki 4 different types ki http request hain. unhe
        //add define kar sake Request call bhejte vakt
        public ApiType ApiType { get; set; } = ApiType.GET;

        /*While sending th request we also need to define the URL*/
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
