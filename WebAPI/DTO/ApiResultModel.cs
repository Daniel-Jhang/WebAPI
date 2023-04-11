namespace WebAPI.DTO
{
    public class ApiResultModel
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorMessageDetail { get; set; }
    }

    public class ApiResultDataModel : ApiResultModel
    {
        public object? Data { get; set; }
    }
}
