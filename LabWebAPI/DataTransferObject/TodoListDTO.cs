namespace LabWebAPI.DataTransferObject
{
    public class TodoListDTO
    {
        public bool Status { get; set; }
        public string Context { get; set; } = null!;
        public bool Editing { get; set; }
    }
}