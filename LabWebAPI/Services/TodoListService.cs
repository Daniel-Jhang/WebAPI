﻿namespace LabWebAPI.Services
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
            var isRecordExist = await _todoListDao.CheckIsExists(context: todoRecord.Context);
            if (isRecordExist)
            {
                _logger.Error("紀錄重複");
                throw new Exception("紀錄重複");
            }
            var recordToCreate = await _todoListDao.CreateTodoRecord(todoRecord);
            return recordToCreate;
        }

        public Task<TodoListDto> GetTodoRecord(string context)
        {
            return _todoListDao.GetTodoRecord(context: context);
        }

        public async Task<List<TodoListDto>> GetAllTodoList()
        {
            return await _todoListDao.GetAllTodoList();
        }

        public async Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord)
        {
            // Business Logic
            var recordToUpdate = await _todoListDao.UpdateTodoRecord(todoRecord);
            return recordToUpdate;
        }

        public Task<bool> ToggleAll(bool status)
        {
            // Business Logic
           return _todoListDao.ToggleAll(status);
        }

        public async Task<List<TodoListDto>> DeleteTodoRecord(string todoRecordId)
        {
            // Business Logic
            var result = await _todoListDao.DeleteTodoRecord(todoRecordId);
            return result;
        }

        public async Task<List<TodoListDto>> ClearCompleted(List<string> completedIdList)
        {
            // Business Logic
          var result = await _todoListDao.ClearCompleted(completedIdList);
            return result;
        }
    }
}
