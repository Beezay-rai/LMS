using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LMS.Models
{
    public class BaseApiResponseModel
    {
        [SwaggerIgnore]
        public HttpStatusCode HttpStatusCode { get; set; }
        public bool Status { get;set; }
        public string Message { get; set; }
    }


    public class ApiResponseModel<T> : BaseApiResponseModel
    {
        public T Data { get; set; }
    }

    public class ApiErrorResponseModel<T> : BaseApiResponseModel
    {
        public List<T> Errors { get; set; } = new List<T>();
    }

 
}
