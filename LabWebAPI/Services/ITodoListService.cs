namespace LabWebAPI.Services
{
    public interface ITodoListService
    {
        Task<TodoListDto> CreateTodoRecord(TodoListDto todoRecord);
        Task<List<TodoListDto>> GetAllTodoList();
        Task<TodoListDto> UpdateTodoRecord(TodoListDto todoRecord);
        Task<bool> ToggleAll(bool status);
        Task<List<TodoListDto>> DeleteTodoRecord(string todoRecordId);
        Task<List<TodoListDto>> ClearCompleted(List<Guid> todoRecordIdList);
    }
}