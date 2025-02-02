namespace LMS.Models
{
    public class ApiResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = new object();
    }

    public class CommonResponseModel<T>
    {
        public bool Status { get; set; }
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
