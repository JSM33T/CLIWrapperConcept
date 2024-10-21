using GUIForCLI.Api.Entities;

namespace GUIForCLI.Services
{
    public interface ICmdService
    {
        public Task<(bool,string)> CustomTask(CLIRequest request);
    }
}
