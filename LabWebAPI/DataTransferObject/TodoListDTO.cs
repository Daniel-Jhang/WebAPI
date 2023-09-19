namespace LabWebAPI.DataTransferObject
{
    public class TodoListDto
    {
        public string TodoId { get; set; } = null!;
        public bool Status { get; set; }
        public string Context { get; set; } = null!;
        public bool Editing { get; set; }
    }
}