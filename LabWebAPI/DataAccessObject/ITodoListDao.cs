namespace LabWebAPI.DataAccessObject
{
    public interface ITodoListDao
    {
        Task<TodoListDto> CreateTodoRecord(TodoListDto newTodo);
        Task<TodoListDto> GetTodoRecord(Guid? todoRecordId = null, string? context = null);
        Task<List<TodoListDto>> GetAllTodoList();
        Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord);
        Task<List<TodoListDto>> DeleteTodoRecord(Guid todoRecordId);
        Task<List<TodoListDto>> ClearCompleted(List<Guid> todoRecordIdList);
    }
}