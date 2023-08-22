namespace LabWebAPI.Controllers
{
    /// <summary>
    /// DONE: 透過appsettings.json中設定連線字串，來切換連線的資料庫(補Medium文章)
    /// TODO: 嘗試用CustomDbConnectionFactory動態切換MSSQL或Oracle資料庫(先嘗試用EF Core，不可行再考慮用Dapper) 提示: builder.Services.AddDbContextFactory<LabContext>();
    /// TODO: 將存取資料的code抽出去，新增WebAPI.Core專案(參考Yata.CustomerApp.Core)
    /// TODO: 串接Angular前端
    /// TODO: 嘗試實作Dao框架
    /// TODO: 嘗試實作Cache
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