namespace LabWebAPI.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListDao _todoListDao;
        private readonly ILogger _logger;

        public TodoListService(ITodoListDao todoListDao, ILogger logger)
        {
            this._todoListDao = todoListDao;
            this._logger = logger;
        }

        public async Task<TodoListDto> CreateTodoRecord(TodoListDto todoRecord)
        {
            var exitTodoRecord = await _todoListDao.GetTodoRecord(context: todoRecord.Context);
            if (exitTodoRecord.TodoId != null)
            {
                _logger.Error("紀錄重複");
                throw new Exception("紀錄重複");
            }
            var recordToCreate = await _todoListDao.CreateTodoRecord(todoRecord);
            return recordToCreate;
        }

        public async Task<List<TodoListDto>> GetAllTodoList()
        {
            return await _todoListDao.GetAllTodoList();
        }

        public async Task<TodoListDto> UpdateProduct(TodoListDto todoRecord)
        {
            // Business Logic
            var recordToUpdate = await _todoListDao.UpdateTodoRecord(todoRecord);
            return recordToUpdate;
        }

        public async Task<List<TodoListDto>> DeleteProduct(Guid todoRecordId)
        {
            // Business Logic
            var result = await _todoListDao.DeleteTodoRecord(todoRecordId);
            return result;
        }
    }
}
