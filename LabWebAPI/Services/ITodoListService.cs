namespace LabWebAPI.Services
{
    public interface ITodoListService
    {
        Task<TodoListDto> CreateTodoRecord(TodoListDto todoRecord);
        Task<List<TodoListDto>> GetAllTodoList();
        Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord);
        Task<List<TodoListDto>> DeleteTodoRecord(Guid todoRecordId);
        Task<List<TodoListDto>> ClearCompleted(List<Guid> todoRecordIdList);
    }
}