using System.Net;
using System.Text.Json.Serialization;

namespace LMS.Models
{
    public class BaseApiResponseModel
    {
        [JsonIgnore]
        public HttpStatusCode HttpStatusCode { get; set; }
        public bool Status { get;set; }
        public string Message { get; set; }
    }


    public class ApiResponseModel<T> : BaseApiResponseModel
    {
        [JsonPropertyOrder(3)]
        public T Data { get; set; }
    }

    public class ApiErrorResponseModel<T> : BaseApiResponseModel
    {
        [JsonPropertyOrder(3)]
        public List<T> Errors { get; set; } = new List<T>();
    }

 
}
