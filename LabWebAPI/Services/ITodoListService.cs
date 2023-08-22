namespace LabWebAPI.Services
{
    public interface ITodoListService
    {
        Task<TodoListDto> CreateTodoRecord(TodoListDto todoRecord);
        Task<List<TodoListDto>> DeleteProduct(Guid todoRecordId);
        Task<List<TodoListDto>> GetAllTodoList();
        Task<TodoListDto> UpdateProduct(TodoListDto todoRecord);
    }
}