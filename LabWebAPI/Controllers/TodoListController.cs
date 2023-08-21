namespace LabWebAPI.Controllers
{
    /// <summary>
    /// TODO: 嘗試用CustomDbConnectionFactory動態切換MSSQL或Oracle資料庫(先嘗試用EF Core，不可行再考慮用Dapper)
    /// TODO: 常適實作Dao框架
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        [HttpPost]
        public async Task<ApiResultModel> Post()
        {
            try
            {
                var result = new ApiResultDataModel();

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorDetail = ex.ToString()
                };
            }
        }

        [HttpGet]
        public async Task<ApiResultModel> Get()
        {
            try
            {
                var result = new ApiResultDataModel();

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorDetail = ex.ToString()
                };
            }
        }

        [HttpPut]
        public async Task<ApiResultModel> Put()
        {
            try
            {
                var result = new ApiResultDataModel();

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorDetail = ex.ToString()
                };
            }
        }

        [HttpDelete]
        public async Task<ApiResultModel> Delete()
        {
            try
            {
                var result = new ApiResultDataModel();

                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorDetail = ex.ToString()
                };
            }
        }
    }
}