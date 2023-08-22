namespace LabWebAPI.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListDao _todoListDao;

        public TodoListService(ITodoListDao todoListDao)
        {
            this._todoListDao = todoListDao;
        }

        public async Task<TodoListDto> CreateTodoRecord(TodoListDto todoRecord)
        {
            var exitTodoRecord = await _todoListDao.GetTodoRecord(context: todoRecord.Context);
            if (exitTodoRecord != null)
            {
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
