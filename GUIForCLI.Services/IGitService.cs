using GUIForCLI.Api.Entities;

namespace GUIForCLI.Services
{
    public interface IGitService
    {
        public Task<string> CustomTask(CLIRequest request);
    }
}
