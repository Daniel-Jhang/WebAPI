namespace LabWebAPI.Controllers
{
    /// <summary>
    /// DONE: 透過appsettings.json中設定連線字串，來切換連線的資料庫(補Medium文章)
    /// DONE: 將DbContext和Table Models抽出去，新增DataAcessLibrary專案
    /// TODO: 嘗試用CustomDbConnectionFactory動態切換MSSQL或Oracle資料庫(先嘗試用EF Core，不可行再考慮用Dapper) 提示: builder.Services.AddDbContextFactory<LabContext>();
    /// TODO: 串接Angular前端
    /// TODO: 嘗試實作Dao框架
    /// TODO: 將存取資料的物件(DAO)移到DataAcessLibrary專案中
    /// TODO: 嘗試實作Cache
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            this._todoListService = todoListService;
        }

        [HttpPost]
        public async Task<ApiResultModel> Post(TodoListDto todoRecord)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _todoListService.CreateTodoRecord(todoRecord);
                result.Data = data;
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
                var data = await _todoListService.GetAllTodoList();
                result.Data = data;
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
        public async Task<ApiResultModel> Put(TodoListDto todoRecord)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _todoListService.UpdateProduct(todoRecord);
                result.Data = data;
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
        public async Task<ApiResultModel> Delete(Guid todoRecordId)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _todoListService.DeleteProduct(todoRecordId);
                result.Data = data;
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