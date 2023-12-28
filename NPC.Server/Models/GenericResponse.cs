namespace NPC.Server.Models
{
    public class GenericResponse
    {
        public int? Status { get; set; } = 0;
        public string? Message { get; set; } = null;
        public object? data { get; set; } = null;
       
    }
}
