namespace LabWebAPI.Controllers
{
    /// <summary>
    /// DONE: 透過appsettings.json中設定連線字串，來切換連線的資料庫(補Medium文章)
    /// DONE: 將DbContext和Table Models抽出去，新增DataAcessLibrary專案
    /// TODO: 串接Angular前端
    /// TODO: 補CORS文章、(可選)前端import toastr套件的方法
    /// TODO: 嘗試用CustomDbConnectionFactory動態切換MSSQL或Oracle資料庫(先嘗試用EF Core，不可行再考慮用Dapper) 提示: builder.Services.AddDbContextFactory<LabContext>();
    /// TODO: 嘗試實作Dao框架
    /// TODO: 將存取資料的物件(DAO)移到DataAcessLibrary專案中
    /// TODO: 嘗試實作Cache
    /// TODO: 研究sqlOptions.EnableRetryOnFailure()和sqlOptions.ExecutionStrategy()
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
        public async Task<ApiResultModel> Get(string? context = null)
        {
            try
            {
                var result = new ApiResultDataModel();
                object data = null!;
                if (!string.IsNullOrEmpty(context))
                {
                    data = await _todoListService.GetTodoRecord(context);
                    result.Data = data;
                    result.IsSuccess = true;
                    return result;
                }
                data = await _todoListService.GetAllTodoList();
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
                var data = await _todoListService.UpdateTodoRecord(todoRecord);
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

        [HttpPut("toggleAll")]
        public async Task<ApiResultModel> ToggleAll(bool status)
        {
            try
            {
                var result = new ApiResultModel();
                var data = await _todoListService.ToggleAll(status);
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
        public async Task<ApiResultModel> Delete(string todoRecordId)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _todoListService.DeleteTodoRecord(todoRecordId);
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

        [HttpDelete("clearCompleted")]
        public async Task<ApiResultModel> ClearCompleted(List<string> completedIdList)
        {
            try
            {
                var result = new ApiResultDataModel();
                var data = await _todoListService.ClearCompleted(completedIdList);
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