namespace LabWebAPI.DataTransferObject
{
    public class ApiResultModel
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorDetail { get; set; } = string.Empty;
    }
    public class ApiResultDataModel : ApiResultModel
    {
        public object? Data { get; set; }
    }
}