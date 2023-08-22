namespace LabWebAPI.DataTransferObject
{
    public class TodoListDto
    {
        public Guid TodoId { get; set; }
        public bool Status { get; set; }
        public string Context { get; set; } = null!;
        public bool Editing { get; set; }
    }
}