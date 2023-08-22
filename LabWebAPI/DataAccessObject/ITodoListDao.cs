namespace LabWebAPI.DataAccessObject
{
    public interface ITodoListDao
    {
        Task<TodoListDto> CreateTodoRecord(TodoListDto newTodo);
        Task<List<TodoListDto>> DeleteTodoRecord(Guid todoRecordId);
        Task<List<TodoListDto>> GetAllTodoList();
        Task<TodoListDto> GetTodoRecord(Guid? todoRecordId = null, string? context = null);
        Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord);
    }
}