using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GUIForCLI.Api.Entities
{
    public class CLIResponse<T>(int status, string message, T data, List<string>? hints = null)
    {
        public int Status { get; set; } = status;
        public string Message { get; set; } = message;
        public List<string> Hints { get; set; } = hints ?? [];
        public T Data { get; set; } = data;
    }
}
