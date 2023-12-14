namespace LabWebAPI.DataAccessObject
{
    public interface ITodoListDao
    {
        Task<TodoListDto> CreateTodoRecord(TodoListDto newTodo);
        Task<TodoListDto> GetTodoRecord(Guid? todoRecordId = null, string? context = null);
        Task<List<TodoListDto>> GetAllTodoList();
        Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord);
        Task<bool> ToggleAll(bool status);
        Task<List<TodoListDto>> DeleteTodoRecord(string todoRecordId);
        Task<List<TodoListDto>> ClearCompleted(List<string> completedIdList);
        Task<bool> CheckIsExists(string? context = null);
    }
}